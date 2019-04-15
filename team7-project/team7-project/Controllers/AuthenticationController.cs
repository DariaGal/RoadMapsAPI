using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Users.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using team7_project.Auth.Tokens;
using team7_project.Errors;
using Models.Users;
using Models.Converters.Users.Models.Converters.Users;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using team7_project.Auth;
using Microsoft.IdentityModel.Tokens;

namespace team7_project.Controllers
{
    [Route("api/auth/signin")]
    [ApiController]
    public class AuthenticationController : Controller
    {
        private readonly IUserService users;

        public AuthenticationController(IUserService users)
        {
            this.users = users;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> GenerateToken([FromBody]Client.Models.Users.UserRegistrationInfo userInfo, [FromServices] IJwtSigningEncodingKey signingEncodingKey, CancellationToken cancellationToken)
        {
            if (userInfo == null)
            {
                var error = ServiceErrorResponses.BodyIsMissing("UserInfo");
                return BadRequest(error);
            }

            if (userInfo.Login == null || userInfo.Password == null)
            {
                var error = ServiceErrorResponses.NotEnoughUserData();
                return BadRequest(error);
            }

            User user;

            try
            {
                user = await users.GetAsync(userInfo.Login, cancellationToken);
            }
            catch (UserNotFoundException)
            {
                var error = ServiceErrorResponses.UserNotFound();
                return BadRequest(error);
            }

            var clientUser = UserConverter.Convert(user);

            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, clientUser.Login),

                new Claim(ClaimTypes.NameIdentifier, clientUser.Id),
        };

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(JWT.GetJWT(claims, signingEncodingKey));

            return Ok(new AuthTokenAnswer
            {
                AccessToken = encodedJwt
            });
        }
    }
}