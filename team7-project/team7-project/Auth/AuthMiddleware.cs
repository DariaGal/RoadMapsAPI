using Models.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace team7_project.Auth
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var authHeader = context.Request.Cookies["auth"];
            if (authHeader == null)
            {
                await _next(context);
            }
            else
            {
                var handler = new JwtSecurityTokenHandler();
                JwtSecurityToken jsonToken;
                try
                {
                    jsonToken = handler.ReadJwtToken(authHeader);
                }catch(ArgumentException)
                { 
                    context.Response.StatusCode = 401;
                    return;
                }

                var login = jsonToken.Claims.First(claim => claim.Type == "Name").Value;
                var id = jsonToken.Claims.First(claim => claim.Type == "Id").Value;

                var connectionString = Environment.GetEnvironmentVariable("CUSTOMCONNSTR_mongoDB", EnvironmentVariableTarget.Process);
                var client = new MongoClient(connectionString);
                var database = client.GetDatabase("team7db");
                var users = database.GetCollection<User>("Users");
                
                var findResults = await users.FindAsync(u => u.Login == login && u.Id == new Guid(id));
                var user = await findResults.FirstOrDefaultAsync();
                if (user == null)
                {
                    context.Response.StatusCode = 401;
                    return;
                }

               
                var claims = new Claim[]
                {
                new Claim("Name", login),
                new Claim("Id", id)
                };

                var identity = new ClaimsIdentity(claims, "basic");
                context.User = new ClaimsPrincipal(identity);

                //Pass to the next middlware
                await _next(context);
            }
        }

    }
}
