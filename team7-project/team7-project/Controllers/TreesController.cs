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

namespace team7_project.Controllers
{
    [Route("api/[controller]")]
    public class TreesController : Controller
    {
        private readonly ITreeService trees;

        public TreesController(ITreeService trees)
        {
            this.trees = trees;
        }

        [AllowAnonymous]
        [HttpGet]
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
                return BadRequest();
            }

            var clientTree = TreeConverter.Convert(tree);

            return Ok(clientTree);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateTreeAsync(CancellationToken cancellationToken)
        {
            var nodes = new List<Node>
            {
                new Node
                {
                     Id = "1",
                     Text = "Доброе Утро",
                     Type = "type",
                     X = 50,
                     Y = 20
                },
                new Node
                {
                     Id = "2",
                     Text = "Славяне",
                     Type = "type",
                     X = 50,
                     Y = 120
                },
                new Node
                {
                     Id = "3",
                     Text = "Флексим",
                     Type = "type",
                     X = 50,
                     Y = 220
                }
            };

            var links = new List<Link>
            {
                new Link
                {
                    Type = "type",
                    SourceId = "1",
                    TargetId ="2"
                },
                new Link
                {
                    Type = "type",
                    SourceId = "2",
                    TargetId ="3"
                }
            };

            var treeID = Guid.NewGuid().ToString();
            var tree = new Models.Trees.Tree
            {
                Id = treeID,
                Title = "Тест",
                Nodes = nodes,
                Links = links,
            };

            await trees.CreateAsync(tree, cancellationToken);

            return Ok(treeID);
        }
    }
}