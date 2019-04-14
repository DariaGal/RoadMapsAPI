using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Models.Users.Services
{
    public interface IUserService
    {
        Task<User> CreateAsync(UserCreationInfo creationInfo, CancellationToken cancellationToken);

        Task<User> GetAsync(Guid userId, CancellationToken cancellationToken);

        Task<User> GetAsync(string login, CancellationToken cancellationToken);
    }
}
