using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Grpc.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using NHibernate.AspNetCore.Identity;
using ProtoBuf.Grpc;
using Course.Data.IRepository;
using Course.Web.Service.Auth;
using Course.Web.Share.Domain;
using Course.Web.Share.IServices;
using Course.Web.Share.Ultils;

namespace Course.Web.Service.Services
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AppUserService : IAppUserService
    {
        readonly ILogger<AppUser> _logger;
        readonly IMapper _mapper;
        readonly UserManager<AppUser> _userManager;
        readonly UserStore<AppUser, AppRole> _userStore;
        readonly IAppUserRepository _userRepository;
        readonly IAuditLogRepository _auditLogRepository;
        readonly SignInManager<AppUser> _signInManager;
        NHibernate.ISession _session;

        public AppUserService(
            ILogger<AppUser> logger,
            IMapper mapper,
            UserManager<AppUser> userManager,
            IUserStore<AppUser> userStore,
            IAppUserRepository userRepository,
            SignInManager<AppUser> signInManager,
            IAuditLogRepository auditLogRepository,
            NHibernate.ISession session
            )
        {
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
            _userStore = (UserStore<AppUser, AppRole>)userStore;
            _userRepository = userRepository;
            _signInManager = signInManager;
            _auditLogRepository = auditLogRepository;
            _session = session;
        }
        public async ValueTask<ExcuteResponse> AddAsync(AppUserData model, CallContext context = default)
        {
            try
            {
                AppUser user = _mapper.Map<AppUser>(model);
                user.NormalizedUserName = model.UserName.ToUpper();
                user.CreateTime = DateTime.Now;


                var result = await _userManager.CreateAsync(user, model.PasswordHash);
                if (result.Succeeded)
                {
                    return new ExcuteResponse() { Id = user.Id, State = true };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return new ExcuteResponse() { State = false };
        }

        public async ValueTask<ExcuteResponse> DeleteAsync(DeleteAppUser appUser, CallContext context = default)
        {
            try
            {
                var user = await _userStore.FindByIdAsync(appUser.Id);
                if (user != null)
                {
                    if (user.NormalizedUserName != "ADMIN" && user.UserName != appUser.VerifyUsername)
                    {
                        var result = await _userStore.DeleteAsync(user);
                        if (result.Succeeded)
                        {
                            return new ExcuteResponse() { Id = user.Id, State = true };
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return new ExcuteResponse() { State = false };
        }
        //[AllowAnonymous]
        public async ValueTask<AppUserData> GetAsync(string userName, CallContext context = default)
        {
            try
            {
                var userCall = context.ServerCallContext.GetHttpContext()?.User;
                var user = await _userStore.FindByNameAsync(userName.ToUpper());
                return _mapper.Map<AppUserData>(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async ValueTask<ExcuteResponse> UpdateAsync(AppUserData model, CallContext context = default)
        {
            try
            {
                var user = await _userStore.FindByIdAsync(model.Id);
                user.Email = model.Email;
                user.DateOfBirth = model.DateOfBirth;
                user.FullName = model.FullName;
                user.JobTitle = model.JobTitle;
                user.IsActive = model.IsActive;
                user.ExpiredDate = model.ExpiredDate;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return new ExcuteResponse() { Id = user.Id, State = true };
                }
            }
            catch (Exception ex) { _logger.LogError(ex, ex.Message); }
            return new ExcuteResponse() { State = false };
        }

        [AllowAnonymous]
        public async ValueTask<SignInResultData> LoginAsync(AppUserData appUser, CallContext context = default)
        {
            try
            {
                SignInResultData result = new()
                {
                    Succeeded = false
                };

                var user = await _userStore.FindByNameAsync(appUser.UserName.ToUpper());
                if (user != null)
                {
                    if (await _signInManager.UserManager.CheckPasswordAsync(user, appUser.PasswordHash))
                    {
                        TokenBuilder tokenBuilder = new TokenBuilder();
                        string token = tokenBuilder.Build(user.UserName, TimeSpan.FromDays(30));
                        return new SignInResultData
                        {
                            Succeeded = true,
                            IsLockedOut = user.LockoutEnabled,
                            IsNotAllowed = !user.IsActive,
                            RequiresTwoFactor = user.TwoFactorEnabled,
                            AuthenToken = token,
                            IsExpired = DateTime.Now > user.ExpiredDate
                        };
                    }
                }
                else
                {
                    if (appUser.UserName.ToUpper() == "ADMIN")
                    {
                        await _userManager.CreateAsync(new AppUser
                        {
                            UserName = "admin",
                            Email = "admin@mail.com",
                            EmailConfirmed = true,
                            FullName = "Admin",
                            IsActive = true,
                            NormalizedEmail = "ADMIN@MAIL.COM",
                            NormalizedUserName = "ADMIN",
                            LockoutEnabled = false
                        }, "Admin123@");
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                return new SignInResultData { Succeeded = false };
            }
        }

        public async ValueTask<ListAppUserResult> GetPageAsync(AppUserSearch searchPage, CallContext context = default)
        {
            try
            {
                var user = await _userRepository
                    .GetPageWithTransactionWithTotalAsync(c => c.UserName.Contains(searchPage.Keyword ?? ""),
                    searchPage.Page.PageIndex - 1,
                    searchPage.Page.PageSize,
                    c => c.UserName,
                    Core.Patterns.Repository.OrderType.Asc);
                return new ListAppUserResult
                {
                    Data = _mapper.Map<List<AppUserData>>(user.Item1),
                    Total = user.Item2
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return null;
            }
        }

        public async ValueTask<ExcuteResponse> ChangePasswordAsync(ChangePasswordAppUser model, CallContext context = default)
        {
            try
            {
                var user = await _userStore.FindByIdAsync(model.Id);
                if (user != null)
                {
                    var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return new ExcuteResponse() { Id = user.Id, State = true };
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
            }
            return new ExcuteResponse() { State = false };
        }

        public async Task<ListAppRoleResult> GetRoleByUserAsync(string id, CallContext context = default)
        {
            try
            {
                var roles = await _userRepository.GetRolesAsync(id);
                return new ListAppRoleResult { Data = _mapper.Map<List<AppRoleData>>(roles) };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task<ListAppRoleResult> ChangeRolesAsync(ChangeRolesData model, CallContext context = default)
        {
            try
            {
                var roles = await _userRepository.ChangeRolesAsync(_mapper.Map<List<AppRole>>(model.Roles), model.UserId);
                return new ListAppRoleResult { Data = _mapper.Map<List<AppRoleData>>(roles) };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        [AllowAnonymous]
        public async Task<ListClaimResult> GetAllClaimsAsync(string userName, CallContext context = default)
        {
            try
            {
                if (userName == null)
                {
                    return null;
                }
                var claims = await _userRepository.GetClaims(userName);
                return new ListClaimResult { Data = claims?.Select(c => new ClaimData { Type = c.ClaimType, Value = c.ClaimValue }).ToList() };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async ValueTask<ExcuteResponse> ResetPasswordAsync(ResetPasswordAppUser model, CallContext context = default)
        {
            try
            {
                var user = await _userStore.FindByIdAsync(model.Id);
                if (user != null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return new ExcuteResponse() { Id = user.Id, State = true };
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
            }
            return new ExcuteResponse() { State = false };
        }

        public async Task<ListClaimResult> GetUserClaimsAsync(string userName, CallContext context = default)
        {
            try
            {
                if (userName == null)
                {
                    return null;
                }
                var claims = await _userRepository.GetUserClaims(userName);
                return new ListClaimResult { Data = claims?.Select(c => new ClaimData { Type = c.ClaimType, Value = c.ClaimValue }).ToList() };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task<ExcuteResponse> UpdateUserClaimsAsync(UpdateUserClaim model, CallContext context = default)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(model.UserName.ToUpper());
                var current = await _userStore.GetClaimsAsync(user);
                var deletedClaims = current.Where(c => !model.PermissionKeys.Contains(c.Value));
                var addClaims = model.PermissionKeys.Where(c => !current.Any(x => x.Value == c)).Select(c => new Claim(CustomClaimTypes.Permission, c));
                if (deletedClaims.Any())
                {
                    await _userStore.RemoveClaimsAsync(user, deletedClaims);
                }
                if (addClaims.Any())
                {
                    await _userStore.AddClaimsAsync(user, addClaims);
                }
                return new ExcuteResponse() { State = true };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return new ExcuteResponse();
        }
    }
}
