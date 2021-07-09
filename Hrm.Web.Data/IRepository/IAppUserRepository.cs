using NHibernate.AspNetCore.Identity;
using Course.Core.Patterns.Repository;
using Course.Web.Share.Domain;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Course.Data.IRepository
{
    public interface IAppUserRepository : IRepository<AppUser>
    {
        Task<List<AppRole>> GetRolesAsync(string userKey);
        Task<List<AppRole>> ChangeRolesAsync(List<AppRole> roleIds, string userKey);
        Task<List<IdentityRoleClaim>> GetClaims(string userName);
        Task<List<IdentityUserClaim>> GetUserClaims(string userName);
    }
}
