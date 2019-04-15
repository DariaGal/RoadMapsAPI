using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models.Converters.Users;
using Models.Converters.Users.Models.Converters.Users;
using Models.Users;
using Models.Users.Services;
using team7_project.Auth;
using team7_project.Auth.Tokens;
using team7_project.Errors;

namespace team7_project.Controllers
{
    [Route("api/auth/signup")]
    public class RegistrationController : Controller
    {
        private readonly IUserService users;

        public RegistrationController(IUserService users)
        {
            this.users = users;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] Client.Models.Users.UserRegistrationInfo registrationInfo, [FromServices] IJwtSigningEncodingKey signingEncodingKey, CancellationToken cancellationToken)
        {
            if (registrationInfo == null)
            {
                var error = ServiceErrorResponses.BodyIsMissing("RegistrationInfo");
                return BadRequest(error);
            }

            if (registrationInfo.Login == null || registrationInfo.Password == null)
            {
                var error = ServiceErrorResponses.NotEnoughUserData();
                return BadRequest(error);
            }

            var creationInfo = new UserCreationInfo(registrationInfo.Login, Auth.AuthHash.GetHashPassword(registrationInfo.Password));

            User user = null;
            try
            {
                user = await users.CreateAsync(creationInfo, cancellationToken);
            }
            catch (UserDuplicationException)
            {
                var error = ServiceErrorResponses.UserNameAlreadyExists(registrationInfo.Login);
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
