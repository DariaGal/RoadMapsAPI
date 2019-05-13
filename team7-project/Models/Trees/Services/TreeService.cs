using Microsoft.Extensions.Configuration;
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

namespace Models.Trees.Services
{
    public class TreeService : ITreeService
    {
        private readonly IMongoCollection<Tree> trees;
        private readonly IMongoCollection<UserTreesCheck> userTreesCheck;

        public TreeService(IConfiguration config)
        {
            var connectionString = Environment.GetEnvironmentVariable("CUSTOMCONNSTR_mongoDB", EnvironmentVariableTarget.Process);
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("team7db");
            trees = database.GetCollection<Tree>("Trees");
            userTreesCheck = database.GetCollection<UserTreesCheck>("UserTrees");
        }

        public async Task<string> CreateAsync(Models.Trees.TreeCreationInfo creationInfo, string authorId, CancellationToken cancellationToken)
        {
            if (creationInfo == null)
            {
                throw new ArgumentNullException(nameof(creationInfo));
            }

            cancellationToken.ThrowIfCancellationRequested();

            var tree = new Tree
            {
                Id = Guid.NewGuid().ToString(),
                AuthorId = authorId,
                Title = creationInfo.Title,
                Description = creationInfo.Description,
                Tags = creationInfo.Tags,
                Links = creationInfo.Links,
                Nodes = creationInfo.Nodes
            };

            InsertOneOptions options = null;
            await trees.InsertOneAsync(tree, options, cancellationToken);

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


        public Task<IReadOnlyList<TreeInfo>> GetAllAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var projection = Builders<Tree>.Projection.Include("Title");
            var result = trees.Find(Builders<Tree>.Filter.Empty).Project(projection).ToList().ToJson();

            var treeInfoList = BsonSerializer.Deserialize<List<TreeInfo>>(result);

            return Task.FromResult<IReadOnlyList<TreeInfo>>(treeInfoList);
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

            if(tree == null)
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
            /*доделать с новым деревом*/

            if(userId == null)
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
            foreach(var treeInfo in userTreesInfo.TreeCkeck)
            {
                treesId.Add(treeInfo.Id);
            }
            
            var filter = new FilterDefinitionBuilder<Tree>().In(x => x.Id, treesId);
            var find = await trees.FindAsync(filter);
            var listOfTree = await find.ToListAsync();
            
            foreach(var tree in listOfTree)
            {
                userTreeInfo.Add(new UserTreeInfo { Id = tree.Id, Title = tree.Title});
            }

            return userTreeInfo;
        }

        public async Task CheckNode(Guid userId, string treeId, string nodeId, CancellationToken cancellationToken)
        {
            if(userId == null)
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
            if(treeCheck == null)
            {
                throw new UserTreeNotFoundException(treeId, userId.ToString());
            }

            var treeFindResult = await trees.FindAsync(t => t.Id == treeId);
            var tree = await treeFindResult.FirstOrDefaultAsync();
            if(tree == null)
            {
                throw new TreeNotFoundException(treeId);
            }

            var node = tree.Nodes.FirstOrDefault(x => x.Id == nodeId);
            if(node == null)
            {
                throw new NodeNotFoundException(nodeId, treeId);
            }

            var update = Builders<UserTreesCheck>.Update.Push(model => model.TreeCkeck[-1].CheckedNodes, nodeId);

            var result = await userTreesCheck.UpdateOneAsync(filter, update);

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

            if(treeInfo == null)
            {
                throw new UserTreeNotFoundException(treeId, userId.ToString());
            }
            
            return treeInfo.CheckedNodes;
        }
    }
}
