using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using NHibernate.AspNetCore.Identity;
using NHibernate.Linq;
using ProtoBuf.Grpc;
using Course.Data.IRepository;
using Course.Web.Share.Domain;
using Course.Web.Share.IServices;
using Course.Web.Share.Ultils;

namespace Course.Web.Service.Services
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AppRoleService : IAppRoleService
    {
        readonly ILogger<AppRole> _logger;
        readonly IMapper _mapper;
        readonly RoleStore<AppRole> _roleStore;
        readonly UserStore<AppUser, AppRole> _userStore;
        readonly IAuditLogRepository _auditLogRepository;
        readonly IAppUserRepository _userRepository;

        public AppRoleService(
            ILogger<AppRole> logger,
            IMapper mapper,
            IRoleStore<AppRole> roleStore,
            IUserStore<AppUser> userStore,
            IAuditLogRepository auditLogRepository,
            IAppUserRepository userRepository
            )
        {
            _logger = logger;
            _mapper = mapper;
            _roleStore = (RoleStore<AppRole>)roleStore;
            _userStore = (UserStore<AppUser, AppRole>)userStore;
            _auditLogRepository = auditLogRepository;
            _userRepository = userRepository;
        }
        public async Task<ExcuteResponse> AddAsync(AppRoleData model, CallContext context = default)
        {
            try
            {
                AppRole role = _mapper.Map<AppRole>(model);

                var result = await _roleStore.CreateAsync(role);
                if (result.Succeeded)
                {
                    return new ExcuteResponse() { Id = role.Id, State = true };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return new ExcuteResponse() { State = false };
        }

        public async Task<ExcuteResponse> DeleteAsync(string modelId, CallContext context = default)
        {
            try
            {
                var role = await _roleStore.FindByIdAsync(modelId);
                if (role != null)
                {
                    var result = await _roleStore.DeleteAsync(role);
                    if (result.Succeeded)
                    {
                        return new ExcuteResponse() { Id = role.Id, State = true };
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return new ExcuteResponse() { State = false };
        }

        public async Task<AppRoleData> GetAsync(string roleName, CallContext context = default)
        {
            try
            {
                var role = await _roleStore.FindByNameAsync(roleName.ToUpper());
                return _mapper.Map<AppRoleData>(role);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<ExcuteResponse> UpdateAsync(AppRoleData model, CallContext context = default)
        {
            try
            {
                var role = await _roleStore.FindByIdAsync(model.Id);
                if (role != null)
                {
                    _mapper.Map(model, role);
                    var result = await _roleStore.UpdateAsync(role);
                    if (result.Succeeded)
                    {
                        return new ExcuteResponse() { Id = role.Id, State = true };
                    }
                }
            }
            catch (Exception)
            {
            }
            return new ExcuteResponse() { State = false };
        }

        public async Task<ListAppRoleResult> GetPageAsync(AppRoleSearch model, CallContext context = default)
        {
            try
            {
                var querySearch = _roleStore.Roles;
                if (!string.IsNullOrEmpty(model.Keyword))
                {
                    querySearch = querySearch.Where(u => u.Name.Contains(model.Keyword));
                }
                var page = querySearch
                    .OrderBy(u => u.Name)
                    .Skip((model.Page.PageIndex - 1) * model.Page.PageSize).Take(model.Page.PageSize).AsEnumerable();
                int total = _roleStore.Roles.Count();
                return await Task.FromResult(new ListAppRoleResult
                {
                    Data = _mapper.Map<List<AppRoleData>>(page.ToList()),
                    Total = total
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task<ListAppRoleResult> GetAllAsync(CallContext context = default)
        {
            try
            {
                var roles = await _roleStore.Roles.ToListAsync();
                return new ListAppRoleResult { Data = _mapper.Map<List<AppRoleData>>(roles) };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task<ListClaimResult> GetAllClaimAsync(AppRoleData model, CallContext context = default)
        {
            try
            {
                var claims = await _roleStore.GetClaimsAsync(_mapper.Map<AppRole>(model));
                return new ListClaimResult { Data = _mapper.Map<List<ClaimData>>(claims.ToList()) };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task<ExcuteResponse> HasUserInRole(string roleName, CallContext context = default)
        {
            try
            {
                var user = await _userStore.GetUsersInRoleAsync(roleName.ToUpper());
                return new ExcuteResponse() { State = user.Any() };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return new ExcuteResponse();
        }

        public async Task<ExcuteResponse> UpdateClaimsAsync(UpdateClaim model, CallContext context = default)
        {
            try
            {
                var appRole = _mapper.Map<AppRole>(model.Role);
                var current = await _roleStore.GetClaimsAsync(appRole);
                var deletedClaims = current.Where(c => !model.PermissionKeys.Contains(c.Value));
                var addClaims = model.PermissionKeys.Where(c => !current.Any(x => x.Value == c)).Select(c => new Claim(CustomClaimTypes.Permission, c));
                if (deletedClaims.Any())
                {
                    foreach (var claim in deletedClaims)
                    {
                        await _roleStore.RemoveClaimAsync(appRole, claim);
                    }
                }
                if (addClaims.Any())
                {
                    foreach (var claim in addClaims)
                    {
                        await _roleStore.AddClaimAsync(appRole, claim);
                    }
                }
                return new ExcuteResponse() { State = true };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return new ExcuteResponse();
        }

        public async Task<ListAppRoleResult> GetByUserAsync(string userId, CallContext context = default)
        {
            try
            {
                var roles = await _userRepository.GetRolesAsync(userId);
                return new ListAppRoleResult { Data = _mapper.Map<List<AppRoleData>>(roles) };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }
    }
}
