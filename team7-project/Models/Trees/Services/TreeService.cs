using Microsoft.Extensions.Configuration;
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
        private readonly IMongoCollection<UserTrees> userTrees;

        public TreeService(IConfiguration config)
        {
            var connectionString = Environment.GetEnvironmentVariable("CUSTOMCONNSTR_mongoDB", EnvironmentVariableTarget.Process);
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("team7db");
            trees = database.GetCollection<Tree>("Trees");
            userTrees = database.GetCollection<UserTrees>("UserTrees");
        }

        public async Task<string> CreateAsync(Models.Trees.TreeCreationInfo creationInfo, CancellationToken cancellationToken)
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





        public async Task AppendTreeToUser(Guid userId, string treeId, CancellationToken cancellationToken)
        {
            var filter = Builders<UserTrees>.Filter.Eq(x => x.UserId, userId);

            var findResult = await userTrees.FindAsync(filter);
            var user = await findResult.FirstOrDefaultAsync();
            if (user == null)
            {
                var uTree = new UserTrees
                {
                    UserId = userId,
                    TreeCkeck = new List<TreeCheckInfo>()
                };

                await userTrees.InsertOneAsync(uTree, cancellationToken: cancellationToken);
            }

            var update = Builders<UserTrees>.Update.AddToSet(
                x => x.TreeCkeck,
                new TreeCheckInfo { Id = treeId, CheckedNodes = new List<string>() }
            );

            var u = await userTrees.FindOneAndUpdateAsync(filter, update);
            return;
        }
    }
}
