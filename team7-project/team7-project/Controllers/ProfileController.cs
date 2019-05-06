using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading;
using Models.Trees.Services;

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

        [HttpPut]
        [Route("trees")]
        public async Task<IActionResult> GetTreesAsync(CancellationToken cancellationToken)
        {
            return Ok();
        }
    }
}