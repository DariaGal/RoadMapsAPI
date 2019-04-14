using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Models.Users.Services
{
    public class UserService : IUserService
    {
        private readonly IMongoCollection<User> users;

        public UserService(IConfiguration config)
        {
            var connectionString = Environment.GetEnvironmentVariable("CUSTOMCONNSTR_mongoDB", EnvironmentVariableTarget.Process);
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("team7db");
            users = database.GetCollection<User>("Users");
        }

        public async Task<User> CreateAsync(UserCreationInfo creationInfo, CancellationToken cancellationToken)
        {
            if (creationInfo == null)
            {
                throw new ArgumentNullException(nameof(creationInfo));
            }

            cancellationToken.ThrowIfCancellationRequested();

            var findResultsDuplicate = await users.FindAsync(u => u.Login == creationInfo.Login, cancellationToken: cancellationToken);
            var userDuplicate = await findResultsDuplicate.FirstOrDefaultAsync(cancellationToken);

            if (userDuplicate != null)
            {
                throw new UserDuplicationException(creationInfo.Login);
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                Login = creationInfo.Login,
                PasswordHash = creationInfo.PasswodHash,
                RegisteredAt = DateTime.UtcNow
            };

            InsertOneOptions options = null;
            await users.InsertOneAsync(user, options, cancellationToken);
            return await Task.FromResult(user);
        }

        public async Task<User> GetAsync(Guid userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var findResults = await users.FindAsync(u => userId == u.Id, cancellationToken: cancellationToken);
            var user = await findResults.FirstOrDefaultAsync(cancellationToken);

            if (user == null)
            {
                throw new UserNotFoundException(userId);
            }

            return user;
        }

        public async Task<User> GetAsync(string login, CancellationToken cancellationToken)
        {
            if (login == null)
            {
                throw new ArgumentNullException(nameof(login));
            }

            cancellationToken.ThrowIfCancellationRequested();

            var findResults = await users.FindAsync(u => u.Login == login, cancellationToken: cancellationToken);
            var user = await findResults.FirstOrDefaultAsync(cancellationToken);

            if (user == null)
            {
                throw new UserNotFoundException(login);
            }

            return user;
        }
    }
}