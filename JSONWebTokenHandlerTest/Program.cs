using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Protocols.WSTrust;
using System.ServiceModel.Security.Tokens;

namespace JSONWebTokenHandlerTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var key = ASCIIEncoding.Unicode.GetBytes("1234567890qwertyuiop");
            var key2 = ASCIIEncoding.Unicode.GetBytes("1234567890qwertyuiop");

            var handler = new JwtSecurityTokenHandler();

            var now = DateTime.UtcNow;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("login", "user1234"),
                    new Claim("realm",  "Realm"),
                }),
                Lifetime = new Lifetime(null, null),
                TokenType = "JWT",
                SigningCredentials = new SigningCredentials(
                        new InMemorySymmetricSecurityKey(key),
                        "http://www.w3.org/2001/04/xmldsig-more#hmac-sha256",
                        "http://www.w3.org/2001/04/xmlenc#sha256"),

            };

            var newToken = handler.CreateToken(tokenDescriptor) as JwtSecurityToken;
            newToken.Payload.Remove("exp");
            newToken.Payload.Remove("nbf");
            var newTokenAsString = handler.WriteToken(newToken);

            handler = new JwtSecurityTokenHandler();

            var validationParameters = new TokenValidationParameters()
            {
                ValidateAudience = false, 
                ValidateLifetime = false,

                ValidateIssuer = true,
                IssuerValidator = (string i, SecurityToken st, TokenValidationParameters vp) => { return i ?? "self"; }, 

                IssuerSigningKey = new InMemorySymmetricSecurityKey(key2),
            };

            SecurityToken securityToken;
            var claims = handler.ValidateToken(newTokenAsString, validationParameters, out securityToken);
        }
    }
}
