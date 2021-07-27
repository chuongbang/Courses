using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Course.Web.Share
{
    public class TokenProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public string AccessToken { get; set; }
        public TokenProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public bool? IsAuthenticated => _httpContextAccessor.HttpContext?.User.Identity.IsAuthenticated;
        public string UserName => _httpContextAccessor.HttpContext?.User.Identity.Name;
    }
}
