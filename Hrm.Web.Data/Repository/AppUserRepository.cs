using Course.Data.IRepository;
using Course.Web.Share.Domain;
using NHibernate;
using NHibernate.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Course.Core.Patterns.Repository;
using System.Security.Claims;
using NHibernate.Linq;

namespace Course.Data.Repository
{
    public class AppUserRepository : Repository<AppUser>, IAppUserRepository
    {
        private IQueryable<IdentityUserRole> UserRoles => session.Query<IdentityUserRole>();
        private IQueryable<IdentityUserClaim> UserClaims => session.Query<IdentityUserClaim>();
        private IQueryable<AppRole> Roles => session.Query<AppRole>();
        public AppUserRepository(ISession session)
            : base(session)
        {
        }

        public async Task<List<AppRole>> GetRolesAsync(string userKey)
        {
            using (var tran = session.BeginTransaction())
            {
                var userId = userKey;
                var query = GetRoles(userKey);
                var roles = query.ToList();
                tran.Commit();
                return await Task.FromResult(roles);
            }
        }
        private IEnumerable<AppRole> GetRoles(string userKey)
        {
            var query = UserRoles.Join(Roles, u => u.RoleId, r => r.Id, (u, r) => new { u, r }).Where(r => r.u.UserId == userKey).Select(r => r.r);
            return query.AsEnumerable();
        }

        public async Task<List<AppRole>> ChangeRolesAsync(List<AppRole> changedRoles, string userKey)
        {
            using (var tran = session.BeginTransaction())
            {
                try
                {
                    var currentRoles = GetRoles(userKey);
                    var changedRoleIds = changedRoles.Select(c => c.Id);
                    var currentRoleIds = currentRoles.Select(c => c.Id);

                    var existRoles = currentRoles.Where(c => changedRoleIds.Contains(c.Id));
                    var newRoles = changedRoles.Where(c => !currentRoleIds.Contains(c.Id));
                    var deleteRoles = currentRoles.Except(existRoles);

                    foreach (var role in deleteRoles)
                    {
                        var userRole = session.Load<IdentityUserRole>(new IdentityUserRole { RoleId = role.Id, UserId = userKey });
                        if (userRole != null)
                            session.Delete(userRole);
                    }
                    foreach (var role in newRoles)
                    {
                        session.Save(new IdentityUserRole { RoleId = role.Id, UserId = userKey });
                    }
                    currentRoles = GetRoles(userKey);
                    tran.Commit();
                    return await Task.FromResult(currentRoles.ToList());
                }
                catch (System.Exception ex)
                {
                    System.Console.WriteLine(ex.StackTrace);
                    tran.Rollback();
                    return await Task.FromResult<List<AppRole>>(null);
                }


            }
        }

        public async Task<List<IdentityRoleClaim>> GetClaims(string userName)
        {
            try
            {
                BeginTransaction();
                var user = await GetAsync(c => c.NormalizedUserName == userName.ToUpper());
                var appRoleIds = await UserRoles.Join(Roles, u => u.RoleId, r => r.Id, (u, r) => new { u, r })
                    .Where(r => r.u.UserId == user.Id).Select(r => r.r.Id).ToListAsync();
                var claims = await session.Query<IdentityRoleClaim>().Where(c => appRoleIds.Contains(c.RoleId)).ToListAsync();
                claims.AddRange(await UserClaims.Where(c => c.UserId == user.Id).Select(c => new IdentityRoleClaim { ClaimType = c.ClaimType, ClaimValue = c.ClaimValue }).ToListAsync());
                await CommitTransactionAsync();
                return claims;
            }
            catch (Exception)
            {
                await RolbackTransactionAsync();
            }
            return new List<IdentityRoleClaim>();
        }

        public async Task<List<IdentityUserClaim>> GetUserClaims(string userName)
        {
            try
            {
                BeginTransaction();
                var user = await GetAsync(c => c.NormalizedUserName == userName.ToUpper());
                var claims = await UserClaims.Where(c => c.UserId == user.Id).ToListAsync();
                await CommitTransactionAsync();
                return claims;
            }
            catch (Exception)
            {
                await RolbackTransactionAsync();
            }
            return new List<IdentityUserClaim>();
        }
    }
}
