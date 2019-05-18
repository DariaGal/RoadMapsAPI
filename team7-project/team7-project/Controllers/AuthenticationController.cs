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
using Microsoft.AspNetCore.Http;

namespace team7_project.Controllers
{
    [Produces("application/json")]
    [Route("api/auth/signin")]
    [ApiController]
    public class AuthenticationController : Controller
    {
        private readonly IUserService users;

        public AuthenticationController(IUserService users)
        {
            this.users = users;
        }


        /// <summary>
        /// Вход
        /// </summary>        
        /// <remarks>
        /// Sample request:
        ///
        ///     POST
        ///     {
        ///        "Login": "user1",
        ///        "Password": "1234"
        ///     }
        ///
        /// </remarks>
        /// <param name="userInfo"> </param> 
        /// <returns>Токен</returns>
        /// <response code="200">Возвращает токен</response>
        /// <response code="400">Если userInfo == null 
        /// Если пароль или логин отсутствует 
        /// Если пользователя не существует
        /// </response>   

        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(AuthTokenAnswer), 200)]
        [ProducesResponseType(typeof(Client.Models.Errors.ServiceErrorResponse), 400)]
        public async Task<IActionResult> GenerateToken(
            [FromBody]Client.Models.Users.UserRegistrationInfo userInfo,
            [FromServices] IJwtSigningEncodingKey signingEncodingKey,
            CancellationToken cancellationToken)
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

            if(!Auth.AuthHash.CheckPassword(userInfo.Password,user.PasswordHash))
            {
                var error = ServiceErrorResponses.WrongPassword();
                return BadRequest(error);
            }

            var clientUser = UserConverter.Convert(user);

            var claims = new Claim[]
            {
                new Claim("Name", clientUser.Login),

                new Claim("Id", clientUser.Id),
        };

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(JWT.GetJWT(claims, signingEncodingKey));
            
            Response.Cookies.Append(
                "auth",
                new AuthTokenAnswer { AccessToken = encodedJwt }.AccessToken,
                new CookieOptions {MaxAge = new TimeSpan(1, 0, 0, 0, 0), SameSite = SameSiteMode.None }
            );

            return Ok(new AuthTokenAnswer
            {
                AccessToken = encodedJwt
            });
        }
    }
}