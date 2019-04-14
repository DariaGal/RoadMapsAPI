using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace team7_project.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    [Route("api/[controller]")]

    public class TestAuthController : Controller
    {
        [HttpGet]
        //[Route("{taskId}", Name = "GetTaskRouteV2")]
        public async Task<IActionResult> GetTaskAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var userLogin = this.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value;



            return this.Ok($"Welcome, {userLogin}!");
        }
    }
}