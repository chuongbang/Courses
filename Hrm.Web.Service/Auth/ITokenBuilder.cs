using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Course.Web.Service.Auth
{
    public interface ITokenBuilder
    {
        string Build(string name, TimeSpan expireTime);
    }
}
