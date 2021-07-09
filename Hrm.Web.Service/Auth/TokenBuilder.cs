using Microsoft.IdentityModel.Tokens;
using Course.Core.Extentions;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Course.Web.Service.Auth
{
    public class TokenBuilder : ITokenBuilder
    {
        public string Build(string name, TimeSpan expireTime)
        {
            var handler = new JwtSecurityTokenHandler();

            var claims = new List<Claim>();
            

            ClaimsIdentity identity = new ClaimsIdentity(
                new GenericIdentity(name, "Bearer"),
                claims
            );

            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = TokenAuthOption.Issuer,
                Audience = TokenAuthOption.Audience,
                SigningCredentials = TokenAuthOption.SigningCredentials,
                Subject = identity,
                Expires = DateTime.Now.AddSeconds(expireTime.TotalSeconds)
            });

            return handler.WriteToken(securityToken);
        }
    }
}
