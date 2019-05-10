using Models.Trees.UserTrees;
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
        Task AppendTreeToUserAsync(Guid userId, string treeId, CancellationToken cancellationToken);

        Task<List<UserTreeInfo>> GetUserTreesAsync(Guid userId, CancellationToken cancellationToken);
        Task CheckNode(Guid userId, string treeId, string nodeId, CancellationToken cancellationToken);
        Task<IReadOnlyList<string>> GetCheckNodes(Guid userId, string treeId, CancellationToken cancellationToken);
    }
}
