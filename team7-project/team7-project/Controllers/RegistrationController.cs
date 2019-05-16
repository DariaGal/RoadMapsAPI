using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
    [Produces("application/json")]
    [Route("api/auth/signup")]
    public class RegistrationController : Controller
    {
        private readonly IUserService users;

        public RegistrationController(IUserService users)
        {
            this.users = users;
        }

        /// <summary>
        /// Регистрация
        /// </summary>        
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /registrationInfo
        ///     {
        ///        "Login": "user1",
        ///        "Password": "1234"
        ///     }
        ///
        /// </remarks>
        /// <param name="registrationInfo"> </param> 
        /// <returns>Токен</returns>
        /// <response code="201">Возвращает токен</response>
        /// <response code="400">Если userInfo == null
        /// Если пароль или логин отсутствует 
        /// Если пользователь уже существует</response> 
        [HttpPost]
        [ProducesResponseType(typeof(AuthTokenAnswer), 201)]
        [ProducesResponseType(typeof(Client.Models.Errors.ServiceErrorResponse), 400)]
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

            if (!AuthValidator.Login(registrationInfo.Login, out var errorLogin))
            {
                var error = ServiceErrorResponses.InvalidLoginOrPassword(errorLogin);
                return BadRequest(error);
            }

            if (!AuthValidator.Password(registrationInfo.Password, out var errorPassword))
            {
                var error = ServiceErrorResponses.InvalidLoginOrPassword(errorPassword);
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
                new Claim("Name", clientUser.Login),

                new Claim("Id", clientUser.Id),
             };

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(JWT.GetJWT(claims, signingEncodingKey));

            Response.Cookies.Append(
                "auth",
                new AuthTokenAnswer { AccessToken = encodedJwt }.AccessToken,
                new CookieOptions { HttpOnly = true }
            );

            return Ok(new AuthTokenAnswer
            {
                AccessToken = encodedJwt
            });
        }
    }
}
