using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Web.Service.Auth
{
    public class TokenAuthOption
    {
        public static string Audience => "HrmAudience";
        public static string Issuer => "HrmIssuer";
        public static RsaSecurityKey Key { get; } = new RsaSecurityKey(RSAKeyHelper.GenerateKey());
        public static SigningCredentials SigningCredentials { get; } = new SigningCredentials(Key, SecurityAlgorithms.RsaSha256Signature);

        public static TimeSpan ExpiresSpan { get; } = TimeSpan.FromDays(30);
        public static string TokenType => "Bearer";
    }
}
