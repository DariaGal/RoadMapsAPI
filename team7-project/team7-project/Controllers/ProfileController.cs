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

        [HttpGet]
        [Route("trees")]
        public async Task<IActionResult> GetTreesAsync(CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(User.Claims.First(c => c.Type == "Id").Value);
            var userLogin = User.Claims.First(c => c.Type == "Name").Value;

            var userTreesModel = await trees.GetUserTreesAsync(userId, cancellationToken);

            var userTrees = UserTreesOutConverter.Convert(userLogin, userTreesModel);

            userTrees.TreesInfo.Where(x => x.Author == userId).ToList().ForEach(y => y.EnableEdit = true);
            return Ok(userTrees);
        }

        [HttpPatch]
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

        [HttpGet]
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

        [HttpPut]
        [Route("trees/{treeId}/edit")]
        public async Task<IActionResult> EditTree([FromRoute] string treeId,/*TreeEditInfo,*/ CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Route("trees/create")]
        public async Task<IActionResult> CreateTree([FromBody] Client.Models.Trees.TreeCreationInfo treeCreationInfo, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}