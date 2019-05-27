using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models.Trees.Services;
using Models.Trees;
using team7_project.Errors;
using Models.Converters.Trees;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Models.Users.Services;

namespace team7_project.Controllers
{
    [Produces("application/json")]
    [Route("api/trees")]
    public class TreesController : Controller
    {
        private readonly ITreeService trees;
        private readonly IUserService users;

        public TreesController(ITreeService trees, IUserService users)
        {
            this.trees = trees;
            this.users = users;
        }

        /// <summary>
        /// Возвращает дерево по id
        /// </summary> 
        /// <param name="treeId"> </param> 
        /// <returns>Дерево</returns>
        /// <response code="200">Возвращает дерево</response>
        /// <response code="400">Если дерево по указанному индексу отсутствует</response>  
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(Client.Models.Trees.Tree), 200)]
        [ProducesResponseType(400)]
        [Route("{treeId}", Name = "GetTreeRoute")]
        public async Task<IActionResult> GetTreeAsync([FromRoute] string treeId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Models.Trees.Tree tree;
            try
            {
                tree = await trees.GetAsync(treeId, cancellationToken);
            }
            catch (TreeNotFoundException)
            {
                var error = ServiceErrorResponses.TreeNotFound(treeId);
                return BadRequest(error);
            }

            var user = await users.GetAsync(tree.AuthorId, cancellationToken);

            var clientTree = TreeConverter.Convert(tree,user.Login);

            return Ok(clientTree);
        }

        /// <summary>
        /// Возвращает список из всех деревьев
        /// </summary>    
        /// <returns>Список информации о деревьях</returns>
        /// <response code="200">Возвращает список информации о деревьях</response>
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<TreeInfo>), 200)]
        [Route("all")]
        public async Task<IActionResult> GetAllTreesAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var info = await trees.GetAllAsync(cancellationToken);

            return Ok(info);
        }


        /// <summary>
        /// Список деревьев соответствующих параметрам поиска
        /// </summary>    
        /// <param name="query"> </param> 
        /// <returns>Список информации о деревьях</returns>
        /// <response code="200">Возвращает список информации о деревьях</response>
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<TreeInfo>), 200)]
        public async Task<IActionResult> SearchTreesAsync([FromQuery]Client.Models.Trees.TreeInfoSearchQuery query,CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var modelQuery = TreeInfoSearchQueryConverter.Convert(query);
            Guid userId = Guid.Empty;
            if (User.Claims.FirstOrDefault() != null)
            {
                userId = Guid.Parse(User.Claims.First(c => c.Type == "Id").Value);
            }
            var info = await trees.SearchTreesAsync(userId, modelQuery, cancellationToken);

            var treeinfo = info.Select(x => TreeInfoConverter.Convert(x));

            return Ok(treeinfo);
        }

        /// <summary>
        /// Добавление дерева в профиль пользователя
        /// </summary> 
        /// <param name="treeId"> </param> 
        /// <response code="200"></response>
        /// <response code="400">Если дерево по указанному индексу отсутствует</response>  
        [Authorize]
        [HttpPatch]
        [Route("{treeID}/add")]
        public async Task<IActionResult> AddTreeToUserAsync([FromRoute] string treeId,CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(User.Claims.First(c => c.Type == "Id").Value);
            try
            {
                await trees.AppendTreeToUserAsync(userId, treeId, cancellationToken);
            }
            catch(TreeNotFoundException)
            {
                var error = ServiceErrorResponses.TreeNotFound(treeId);
                return BadRequest(error);
            }

            return Ok();
        }
    }
}