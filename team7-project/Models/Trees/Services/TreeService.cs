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
        public TreeService(IConfiguration config)
        {
            var connectionString = Environment.GetEnvironmentVariable("CUSTOMCONNSTR_mongoDB", EnvironmentVariableTarget.Process);
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("team7db");
            trees = database.GetCollection<Tree>("Trees");
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
            if(id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            cancellationToken.ThrowIfCancellationRequested();

            var treeFound = trees.Find(x => x.Id == id);
            var tree = treeFound.FirstOrDefault();

            if(tree == null)
            {
                throw new TreeNotFoundException(id);
            }

            return Task.FromResult(tree);
        }

        public Task CreateAsync(Tree tree, CancellationToken cancellationToken)
        {
            if (tree == null)
            {
                throw new ArgumentNullException(nameof(tree));
            }

            trees.InsertOne(tree);

            return Task.CompletedTask;
        }
    }
}
