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
using System.Security.Claims;

namespace Course.Web.Client.Service
{
    public class AppRoleAdapterService : BaseAdapterService<IAppRoleService>
    {
        public AppRoleAdapterService(HostService host, FileLoggerProvider fileLoggerProvider, TokenProvider tokenProvider)
            : base(host, fileLoggerProvider, tokenProvider)
        {
        }

        public async Task<AppRoleData> GetAsync(string roleName)
        {
            try
            {
                return await Service.GetAsync(roleName);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<AppRoleData>> GetAllAsync()
        {
            return (await Service.GetAllAsync()).Data;
        }

        public async Task<ListAppRoleResult> GetPageAsync(string search, Page page)
        {
            try
            {
                return await Service.GetPageAsync(new AppRoleSearch { Page = page, Keyword = search });
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<ExcuteResponse> UpdateAsync(AppRoleData model)
        {
            return await Service.UpdateAsync(model);
        }

        public async Task<ExcuteResponse> AddAsync(AppRoleData model)
        {
            return await Service.AddAsync(model);
        }

        public async Task<ExcuteResponse> DeleteAsync(string modelId)
        {
            return await Service.DeleteAsync(modelId);
        }

        public async Task<List<ClaimData>> GetAllClaimAsync(AppRoleData model)
        {
            var result = await Service.GetAllClaimAsync(model);
            return result?.Data ?? new List<ClaimData>();
        }

        public async Task<ExcuteResponse> HasUserInRole(string roleName)
        {
            return await Service.HasUserInRole(roleName);
        }

        public async Task<ExcuteResponse> UpdateClaimsAsync(AppRoleData model, List<string> permissionKeys)
        {
            return await Service.UpdateClaimsAsync(new UpdateClaim
            {
                PermissionKeys = permissionKeys,
                Role = model
            });
        }
    }

    public class RolesFilter
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string RoleName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string IsActive { get; set; }
        public string IdRole { get; set; }
    }

}
