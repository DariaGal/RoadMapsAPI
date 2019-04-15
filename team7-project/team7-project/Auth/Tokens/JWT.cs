using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using team7_project.Auth.Tokens;

namespace team7_project.Auth
{
    public class JWT
    {
        public static JwtSecurityToken GetJWT(Claim[] claims, IJwtSigningEncodingKey signingEncodingKey)
        {
            return new JwtSecurityToken(
                //issuer: "team7_projectApp",
                //audience: "team7_projectClient",
                claims: claims,
                expires: DateTime.Now.AddMinutes(AuthOptions.LIFETIME),
                signingCredentials: new SigningCredentials(signingEncodingKey.GetKey(), signingEncodingKey.SigningAlgorithm)
            );
        }
    }
}
