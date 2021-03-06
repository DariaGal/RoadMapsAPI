﻿using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using MongoDB.Driver.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using Models.Trees.UserTrees;
using Models.Users;
using System.Security;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Models.Trees.Services
{
    public class TreeService : ITreeService
    {
        private readonly IMongoCollection<Tree> trees;
        private readonly IMongoCollection<UserTreesCheck> userTreesCheck;
        private readonly IMongoCollection<User> users;

        public TreeService(IConfiguration config)
        {
            var connectionString = Environment.GetEnvironmentVariable("CUSTOMCONNSTR_mongoDB", EnvironmentVariableTarget.Process);
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("team7db");
            trees = database.GetCollection<Tree>("Trees");
            userTreesCheck = database.GetCollection<UserTreesCheck>("UserTrees");
            users = database.GetCollection<User>("Users");
        }

        public async Task<string> CreateAsync(Models.Trees.TreeCreationInfo creationInfo, Guid authorId, CancellationToken cancellationToken)
        {
            if (creationInfo == null)
            {
                throw new ArgumentNullException(nameof(creationInfo));
            }

            cancellationToken.ThrowIfCancellationRequested();

            var treeId = Guid.NewGuid().ToString();
            var tree = new Tree
            {
                Id = treeId,
                AuthorId = authorId,
                Title = creationInfo.Title,
                Description = creationInfo.Description,
                Tags = creationInfo.Tags,
                Links = creationInfo.Links,
                Nodes = creationInfo.Nodes
            };

            InsertOneOptions options = null;
            await trees.InsertOneAsync(tree, options, cancellationToken);

            await AppendTreeToUserAsync(authorId, treeId, cancellationToken);
            return tree.Id;
        }


        public async Task<Tree> GetAsync(string id, CancellationToken cancellationToken)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            cancellationToken.ThrowIfCancellationRequested();

            var findResults = await trees.FindAsync(t => t.Id == id, cancellationToken: cancellationToken);
            var tree = await findResults.FirstOrDefaultAsync(cancellationToken);

            if (tree == null)
            {
                throw new TreeNotFoundException(id);
            }

            return tree;
        }


        public async Task<IReadOnlyList<TreeInfo>> GetAllAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var findResultTrees = await trees.FindAsync(Builders<Tree>.Filter.Empty);
            var allTrees = await findResultTrees.ToListAsync();

            var findResultUsers = await users.FindAsync(Builders<User>.Filter.Empty);
            var allUsers = await findResultUsers.ToListAsync();

            var treeInfoList = new List<TreeInfo>();

            foreach (var tree in allTrees)
            {
                var author = allUsers.Find(x => x.Id == tree.AuthorId);
                var treeinfo = new TreeInfo
                {
                    Id = tree.Id,
                    Author = author.Login,
                    Description = tree.Description,
                    Tags = tree.Tags,
                    Title = tree.Title
                };

                treeInfoList.Add(treeinfo);
            }

            return treeInfoList;
        }

        public async Task AppendTreeToUserAsync(Guid userId, string treeId, CancellationToken cancellationToken)
        {
            var filter = Builders<UserTreesCheck>.Filter.Eq(x => x.UserId, userId);

            var findResult = await userTreesCheck.FindAsync(filter);
            var user = await findResult.FirstOrDefaultAsync();
            if (user == null)
            {
                var uTree = new UserTreesCheck
                {
                    UserId = userId,
                    TreeCkeck = new List<TreeCheckInfo>()
                };

                await userTreesCheck.InsertOneAsync(uTree, cancellationToken: cancellationToken);
            }

            var treeFindResult = await trees.FindAsync(t => t.Id == treeId);
            var tree = await treeFindResult.FirstOrDefaultAsync();

            if (tree == null)
            {
                throw new TreeNotFoundException(treeId);
            }

            var update = Builders<UserTreesCheck>.Update.AddToSet(
                x => x.TreeCkeck,
                new TreeCheckInfo { Id = treeId, CheckedNodes = new List<string>() }
            );

            var u = await userTreesCheck.FindOneAndUpdateAsync(filter, update);
            return;
        }

        public async Task<List<UserTreeInfo>> GetUserTreesAsync(Guid userId, CancellationToken cancellationToken)
        {
            if (userId == null)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            cancellationToken.ThrowIfCancellationRequested();

            var userTreeInfo = new List<UserTreeInfo>();

            var findResult = await userTreesCheck.FindAsync(u => u.UserId == userId);
            var userTreesInfo = await findResult.FirstOrDefaultAsync();

            if (userTreesInfo == null)
            {
                var uTree = new UserTreesCheck
                {
                    UserId = userId,
                    TreeCkeck = new List<TreeCheckInfo>()
                };

                await userTreesCheck.InsertOneAsync(uTree, cancellationToken: cancellationToken);

                return userTreeInfo;
            }

            var treesId = new List<string>();
            foreach (var treeInfo in userTreesInfo.TreeCkeck)
            {
                treesId.Add(treeInfo.Id);
            }

            var filter = new FilterDefinitionBuilder<Tree>().In(x => x.Id, treesId);
            var find = await trees.FindAsync(filter);
            var listOfTree = await find.ToListAsync();

            var findResultUsers = await users.FindAsync(Builders<User>.Filter.Empty);
            var allUsers = await findResultUsers.ToListAsync();

            foreach (var tree in listOfTree)
            {
                var author = allUsers.Find(x => x.Id == tree.AuthorId);
                var enableEdit = userId == author.Id;

                var checkedNodes = userTreesInfo.TreeCkeck.FirstOrDefault(x => x.Id == tree.Id).CheckedNodes;
                var nodeCount = tree.Nodes.Count;
                userTreeInfo.Add(
                    new UserTreeInfo
                    {
                        Id = tree.Id,
                        Author = author.Login,
                        Title = tree.Title,
                        Description = tree.Description,
                        Tags = tree.Tags,
                        EnableEdit = enableEdit,
                        CheckedNodes = checkedNodes,
                        AllNodesCount = nodeCount
                    });
            }

            return userTreeInfo;
        }

        public async Task CheckNode(Guid userId, string treeId, string nodeId, CancellationToken cancellationToken)
        {
            if (userId == null)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            if (treeId == null)
            {
                throw new ArgumentNullException(nameof(treeId));
            }

            if (nodeId == null)
            {
                throw new ArgumentNullException(nameof(nodeId));
            }

            cancellationToken.ThrowIfCancellationRequested();

            var filterUser = Builders<UserTreesCheck>.Filter.Eq(u => u.UserId, userId);
            var filterTree = Builders<UserTreesCheck>.Filter.ElemMatch(x => x.TreeCkeck, y => y.Id == treeId);
            var filter = filterUser & filterTree;

            var userTreesFindResult = await userTreesCheck.FindAsync(filterUser);
            var userTrees = await userTreesFindResult.FirstOrDefaultAsync();

            var treeCheck = userTrees.TreeCkeck.FirstOrDefault(t => t.Id == treeId);
            if (treeCheck == null)
            {
                throw new UserTreeNotFoundException(treeId, userId.ToString());
            }

            var treeFindResult = await trees.FindAsync(t => t.Id == treeId);
            var tree = await treeFindResult.FirstOrDefaultAsync();
            if (tree == null)
            {
                throw new TreeNotFoundException(treeId);
            }

            var node = tree.Nodes.FirstOrDefault(x => x.Id == nodeId);
            if (node == null)
            {
                throw new NodeNotFoundException(nodeId, treeId);
            }

            if (treeCheck.CheckedNodes.Contains(nodeId))
            {
                treeCheck.CheckedNodes.Remove(nodeId);
                var pull = Builders<UserTreesCheck>.Update.PullFilter(x => x.TreeCkeck, t => t.Id == treeId);
                var push = Builders<UserTreesCheck>.Update.Push(x => x.TreeCkeck, treeCheck);
                var resPull = await userTreesCheck.UpdateOneAsync(filterUser, pull);
                var resPush = await userTreesCheck.UpdateOneAsync(filterUser, push);
            }
            else
            {
                var update = Builders<UserTreesCheck>.Update.Push(model => model.TreeCkeck[-1].CheckedNodes, nodeId);

                var result = await userTreesCheck.UpdateOneAsync(filter, update);
            }
            return;
        }

        public async Task<IReadOnlyList<string>> GetCheckNodes(Guid userId, string treeId, CancellationToken cancellationToken)
        {
            if (userId == null)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            if (treeId == null)
            {
                throw new ArgumentNullException(nameof(treeId));
            }

            var treeInfo = await userTreesCheck.AsQueryable()
                .Where(x => x.UserId == userId)
                .SelectMany(x => x.TreeCkeck)
                .Where(x => x.Id == treeId)
                .FirstOrDefaultAsync();

            if (treeInfo == null)
            {
                throw new UserTreeNotFoundException(treeId, userId.ToString());
            }

            return treeInfo.CheckedNodes;
        }

        public async Task UpdateTreeAsync(string treeId, Guid userId, TreeCreationInfo treeEditInfo, CancellationToken cancellationToken)
        {
            var findTreeResult = await trees.FindAsync(x => x.Id == treeId);
            var tree = await findTreeResult.FirstOrDefaultAsync();

            if (userId != tree.AuthorId)
            {
                throw new UserDoesNotHavePermissionToEditTree(treeId, userId.ToString());
            }

            var newTree = new Tree
            {
                Id = treeId,
                AuthorId = tree.AuthorId,
                Description = treeEditInfo.Description,
                Title = treeEditInfo.Title,
                Tags = treeEditInfo.Tags,
                Nodes = treeEditInfo.Nodes,
                Links = treeEditInfo.Links
            };

            await trees.FindOneAndReplaceAsync(x => x.Id == treeId, newTree, cancellationToken: cancellationToken);
        }

        public async Task<IReadOnlyList<TreeInfo>> SearchTreesAsync(Guid userId, TreeInfoSearchQuery query, CancellationToken cancellationToken)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            cancellationToken.ThrowIfCancellationRequested();

            FilterDefinition<Tree> filter = FilterDefinition<Tree>.Empty;

            if (query.Tags != null)
            {
                filter = filter & Builders<Tree>.Filter.AnyIn(x => x.Tags, query.Tags);
            }

            if (query.Title != null)
            {
                var pattern = @"\w*" + query.Title + @"\w*";
                var regex = new Regex(pattern, RegexOptions.IgnoreCase);
                filter = filter & Builders<Tree>.Filter.Regex(x => x.Title, new BsonRegularExpression(regex));
            }

            var findResultTrees = await trees.FindAsync(filter);
            var treesList = await findResultTrees.ToListAsync();

            if (query.Offset != null)
            {
                treesList.Skip(query.Offset.Value);
            }

            if (query.Limit != null)
            {
                treesList.Take(query.Limit.Value);
            }

            var findResultUsers = await users.FindAsync(Builders<User>.Filter.Empty);
            var usersList = await findResultUsers.ToListAsync();
        
           
            var findResultUserTrees = await userTreesCheck.FindAsync(x => x.UserId == userId);
            var usertrees = await findResultUserTrees.FirstOrDefaultAsync();
            

            var treesInfoList = new List<TreeInfo>();

            foreach (var tree in treesList)
            {
                var author = usersList.Find(x => x.Id == tree.AuthorId);
                var addedToProfile = false;

                if(usertrees != null)
                {
                    addedToProfile = usertrees.TreeCkeck.FirstOrDefault(x => x.Id == tree.Id) != null;
                }

                treesInfoList.Add(
                    new TreeInfo
                    {
                        Id = tree.Id,
                        Author = author.Login,
                        Title = tree.Title,
                        Description = tree.Description,
                        Tags = tree.Tags,
                        AddedToProfile = addedToProfile
                        
                    });
            }
            return treesInfoList;
        }

        public async Task RemoveTreeAsync(string treeId, Guid userId, CancellationToken cancellationToken)
        {
            if (treeId == null)
            {
                throw new ArgumentNullException(nameof(treeId));
            }

            var findTreeResult = await trees.FindAsync(x => x.Id == treeId);
            var tree = await findTreeResult.FirstOrDefaultAsync();

            if (tree == null)
            {
                throw new TreeNotFoundException(treeId);
            }

            if (userId != tree.AuthorId)
            {
                throw new UserDoesNotHavePermissionToEditTree(treeId, userId.ToString());
            }

            var update = Builders<UserTreesCheck>.Update.PullFilter(x => x.TreeCkeck, f => f.Id == treeId);

            await userTreesCheck.UpdateManyAsync(FilterDefinition<UserTreesCheck>.Empty, update);

            await trees.FindOneAndDeleteAsync(t => t.Id == treeId);
        }

        public async Task RemoveTreeFromProfileAsync(string treeId, Guid userId, CancellationToken cancellationToken)
        {
            if (treeId == null)
            {
                throw new ArgumentNullException(nameof(treeId));
            }

            var findTreeResult = await trees.FindAsync(x => x.Id == treeId);
            var tree = await findTreeResult.FirstOrDefaultAsync();

            if (tree == null)
            {
                throw new TreeNotFoundException(treeId);
            }

            if (userId != tree.AuthorId)
            {
                throw new UserDoesNotHavePermissionToEditTree(treeId, userId.ToString());
            }

            await trees.FindOneAndDeleteAsync(t => t.Id == treeId);
        }
    }
}
