using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Models.Trees.Services
{
    public interface ITreeService
    {
        Task<Tree> GetAsync(string id, CancellationToken cancellationToken);
        Task<IReadOnlyList<TreeInfo>> GetAllAsync(CancellationToken cancellationToken);
        Task<string> CreateAsync(Models.Trees.TreeCreationInfo creationInfo, CancellationToken cancellationToken);
    }
}
