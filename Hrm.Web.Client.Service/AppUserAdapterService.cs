using Course.Data.IRepository;
using Course.Web.Share.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using NHibernate.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Course.Web.Share;
using Course.Core.AuditLog;
using Course.Web.Share.IServices;

namespace Course.Web.Client.Service
{
    public class AppUserAdapterService : BaseAdapterService<IAppUserService>
    {
        public AppUserAdapterService(HostService host, FileLoggerProvider fileLoggerProvider, TokenProvider tokenProvider)
            : base(host, fileLoggerProvider, tokenProvider)
        {
        }

        public async Task<SignInResultData> LoginAsync(string userName, string password)
        {
            return await Service.LoginAsync(new AppUserData { UserName = userName, PasswordHash = password });
        }

        public async Task<AppUserData> GetByUserNameAsync(string userName)
        {
            return await Service.GetAsync(userName);
        }

        public async Task<ListAppUserResult> GetPageAsync(string search, Page page)
        {
            return await Service.GetPageAsync(new AppUserSearch { Page = page, Keyword = search });
        }

        public async Task<ExcuteResponse> UpdateAsync(AppUserData model)
        {
            return await Service.UpdateAsync(model);
        }

        public async Task<ExcuteResponse> AddAsync(AppUserData model)
        {
            return await Service.AddAsync(model);
        }

        public async Task<ExcuteResponse> DeleteAsync(AppUserData model, string verifyUsername)
        {
            try
            {
                return await Service.DeleteAsync(new DeleteAppUser { Id = model.Id, VerifyUsername = verifyUsername });
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ExcuteResponse> ChangePasswordAsync(string userId, string oldPassword, string newPassword)
        {
            try
            {
                return await Service.ChangePasswordAsync(new ChangePasswordAppUser
                {
                    NewPassword = newPassword,
                    OldPassword = oldPassword,
                    Id = userId
                });
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<AppRoleData>> GetRoleByUserAsync(string id)
        {
            return (await Service.GetRoleByUserAsync(id)).Data;
        }

        public async Task<List<AppRoleData>> ChangeRolesAsync(List<AppRoleData> changedRoles, string userId)
        {
            return (await Service.ChangeRolesAsync(new ChangeRolesData { Roles = changedRoles, UserId = userId })).Data;
        }

        public async Task<List<ClaimData>> GetAllClaimsAsync(string userName)
        {
            var result = await Service.GetAllClaimsAsync(userName);
            return result?.Data ?? new List<ClaimData>();
        }

        public async Task<List<ClaimData>> GetUserClaimsAsync(string userName)
        {
            var result = await Service.GetUserClaimsAsync(userName);
            return result?.Data ?? new List<ClaimData>();
        }

        public async Task<ExcuteResponse> ResetPasswordAsync(string userId, string newPassword)
        {
            return await Service.ResetPasswordAsync(new ResetPasswordAppUser
            {
                NewPassword = newPassword,
                Id = userId
            });
        }

        public async Task<ExcuteResponse> UpdateUserClaimsAsync(string userName, List<string> permissionKeys)
        {
            return await Service.UpdateUserClaimsAsync(new UpdateUserClaim
            {
                PermissionKeys = permissionKeys,
                UserName = userName
            });
        }

    }

    public class UsersFilter
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string IsActive { get; set; }
        public string IdRole { get; set; }
    }

}
