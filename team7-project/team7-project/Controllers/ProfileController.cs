using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading;
using Models.Trees.Services;
using Models.Converters.Trees;
using Models.Trees;
using team7_project.Errors;
using Microsoft.AspNetCore.Http;
using Client.Models.Trees.UserTrees;

namespace team7_project.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/profile")]
    public class ProfileController : Controller
    {
        private readonly ITreeService trees;
        public ProfileController(ITreeService trees)
        {
            this.trees = trees;
        }

        /// <summary>
        /// Возвращает список деревьев пользователя
        /// </summary>  
        /// <returns>Список информации о деревьях</returns>
        /// <response code="200">Список информации о деревьях</response>
        [HttpGet]
        [ProducesResponseType(typeof(UserTreesOut), 200)]
        [Route("")]
        public async Task<IActionResult> GetTreesAsync(CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(User.Claims.First(c => c.Type == "Id").Value);
            var userLogin = User.Claims.First(c => c.Type == "Name").Value;

            var userTreesModel = await trees.GetUserTreesAsync(userId, cancellationToken);
            var userTrees = UserTreesOutConverter.Convert(userLogin, userTreesModel);

            return Ok(userTrees);
        }

        /// <summary>
        /// Добавляет вершину дерева в список помеченных вершин
        /// </summary> 
        /// <param name="treeId"> </param> 
        /// <param name="checknode"> </param> 
        /// <response code="200"></response>
        /// <response code="400">Если пользователь не добавлял дерево себе в профиль
        /// Если дерева не существует
        /// Если вершины не существует</response> 
        [HttpPatch]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(Client.Models.Errors.ServiceErrorResponse), 400)]
        [Route("trees/{treeId}")]
        public async Task<IActionResult> PutCheckNode([FromRoute] string treeId, [FromQuery] string checknode, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(User.Claims.First(c => c.Type == "Id").Value);
            try
            {
                await trees.CheckNode(userId, treeId, checknode, cancellationToken);
            }
            catch (UserTreeNotFoundException)
            {
                var error = ServiceErrorResponses.UserTreeNotFound(treeId, userId.ToString());
                return BadRequest(error);
            }
            catch (TreeNotFoundException)
            {
                var error = ServiceErrorResponses.TreeNotFound(treeId);
                return BadRequest(error);
            }
            catch (NodeNotFoundException)
            {
                var error = ServiceErrorResponses.NodeNotFound(checknode, treeId);
                return BadRequest(error);
            }

            return Ok();
        }


        /// <summary>
        /// Возвращает список помеченных вершин дерева
        /// </summary> 
        /// <param name="treeId"> </param>
        /// <response code="200"></response>
        /// <response code="400">Если пользователь не добавлял дерево себе в профиль</response> 
        [HttpGet]
        [ProducesResponseType(typeof(List<string>),200)]
        [ProducesResponseType(typeof(Client.Models.Errors.ServiceErrorResponse), 400)]
        [Route("trees/{treeId}/checknodes")]
        public async Task<IActionResult> GetCheckNodes([FromRoute] string treeId, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(User.Claims.First(c => c.Type == "Id").Value);
            IReadOnlyList<string> checkNodes;
            try
            {
                checkNodes = await trees.GetCheckNodes(userId, treeId, cancellationToken);
            }
            catch (UserTreeNotFoundException)
            {
                var error = ServiceErrorResponses.UserTreeNotFound(treeId, userId.ToString());
                return BadRequest(error);
            }
            return Ok(checkNodes);
        }

        /// <summary>
        /// Редактирование дерева
        /// </summary> 
        /// <param name="treeId"> </param>
        /// <param name="treeEditInfo"> </param>
        /// <response code="200"></response>
        /// <response code="403">Если пользователь не является автором дерева</response> 
        [HttpPut
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(Client.Models.Errors.ServiceErrorResponse), 403)]
        [Route("trees/{treeId}/edit")]
        public async Task<IActionResult> EditTree([FromRoute] string treeId, [FromBody] Client.Models.Trees.TreeCreationInfo treeEditInfo, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(User.Claims.First(c => c.Type == "Id").Value);
            
            var tree = TreeCreationInfoConverter.Convert(treeEditInfo);

            try
            {
                await trees.UpdateTreeAsync(treeId, userId, tree, cancellationToken);
            }catch(UserDoesNotHavePermissionToEditTree)
            {
                var error = ServiceErrorResponses.UserCannotEditTree(treeId, userId.ToString());
                return StatusCode(StatusCodes.Status403Forbidden, error);
            }

            return Ok();
        }
        
        /// <summary>
        /// Создание дерева
        /// </summary>  
        /// <param name="treeCreationInfo"> </param> 
        /// <returns>Id дерева</returns>
        /// <response code="201">Возвращает Id созданного дерева</response>
        [HttpPost]
        [ProducesResponseType(typeof(string), 201)]
        [Route("create")]
        public async Task<IActionResult> CreateTreeAsync([FromBody] Client.Models.Trees.TreeCreationInfo treeCreationInfo, CancellationToken cancellationToken)
        {
            //TODO: добавить всяккие проверочки на адекватность данных, но это потом

            var authorId = Guid.Parse(User.Claims.First(c => c.Type == "Id").Value);

            var tree = TreeCreationInfoConverter.Convert(treeCreationInfo);
            var treeId = await trees.CreateAsync(tree, authorId, cancellationToken);
            
            var routeParams = new Dictionary<string, object>
            {
                { "treeId", treeId }
            };

            return CreatedAtRoute("GetTreeRoute", routeParams, treeId);
        }
    }
}