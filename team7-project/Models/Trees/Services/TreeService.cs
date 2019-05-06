﻿using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;

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

        public Task<IReadOnlyList<TreeInfo>> GetAllAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var projection = Builders<Tree>.Projection.Include("Title");
            var result = trees.Find(Builders<Tree>.Filter.Empty).Project(projection).ToList().ToJson();

            var treeInfoList = BsonSerializer.Deserialize<List<TreeInfo>>(result);

            return Task.FromResult<IReadOnlyList<TreeInfo>>(treeInfoList);
        }

        public Task<Tree> GetAsync(string id, CancellationToken cancellationToken)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            cancellationToken.ThrowIfCancellationRequested();

            var treeFound = trees.Find(x => x.Id == id);
            var tree = treeFound.FirstOrDefault();

            if (tree == null)
            {
                throw new TreeNotFoundException(id);
            }

            return Task.FromResult(tree);
        }

        public Task<string> CreateAsync(Models.Trees.TreeCreationInfo creationInfo, CancellationToken cancellationToken)
        {
            if (creationInfo == null)
            {
                throw new ArgumentNullException(nameof(creationInfo));
            }

            cancellationToken.ThrowIfCancellationRequested();

            var tree = new Tree
            {
                Id = Guid.NewGuid().ToString(),
                Title = creationInfo.Title,
                Links = creationInfo.Links,
                Nodes = creationInfo.Nodes
            };

            trees.InsertOne(tree);

            return Task.FromResult(tree.Id);
        }

        public async Task AppendTreeToUser(Guid userId, string treeId, CancellationToken cancellationToken)
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

            var update = Builders<UserTreesCheck>.Update.AddToSet(
                x => x.TreeCkeck,
                new TreeCheckInfo { Id = treeId, CheckedNodes = new List<string>() }
            );

            var u = await userTreesCheck.FindOneAndUpdateAsync(filter, update);
            return;
        }

        public async Task GetUserTreesAsync(Guid userId, CancellationToken cancellationToken)
        {

        }
    }
}