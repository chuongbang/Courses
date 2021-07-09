using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Course.Core.Extentions;
using Course.Web.Client.Models;
using Course.Core.AuditLog;
using Course.Web.Client.Service;
using Microsoft.AspNetCore.Identity;
using Course.Web.Client.Pages.TaiKhoan;
using Course.Web.Share.Ultils;

namespace Course.Web.Client
{
    public partial class LoginModel : PageModel
    {
        public string ReturnUrl { get; set; } = "~/";
        protected LoginInputModel loginModels;
        private readonly ILogger<LoginInputModel> _logger;
        private readonly AppUserAdapterService _userService;
        public LoginModel(FileLoggerProvider fileLoggerProvider, AppUserAdapterService userService)
        {
            _logger = fileLoggerProvider.CreateLogger<LoginInputModel>();
            _userService = userService;
        }
        [BindProperty] public LoginInputModel Input { get; set; }
        public string Message { get; private set; }

        public IActionResult OnGet(string returnUrl = null)
        {
            if (returnUrl != null)
            {
                ReturnUrl = returnUrl;
            }
            if (HttpContext.User != null && HttpContext.User.Identity.IsAuthenticated)
            {
                return LocalRedirect(ReturnUrl);
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl)
        {
            Message = null;
            if (returnUrl != null)
            {
                ReturnUrl = returnUrl;
            }
            if (ModelState.IsValid)
            {
                var result = await LoginAsync(Input.UserName.Trim(), Input.Password.Trim(), Input.IsRemember);
                if (result.Item1)
                {
                    _logger.LogInformation(Input.UserName + " logged in.");
                    return LocalRedirect(ReturnUrl);
                }
                else
                {
                    Message = result.Item2;
                    ModelState.AddModelError(string.Empty, result.Item2);
                }
            }
            return Page();
        }

        private async Task<(bool, string)> LoginAsync(string userName, string password, bool isRemember)
        {
            try
            {
                if (userName.IsNotNullOrEmpty() && password.IsNotNullOrEmpty())
                {
                    var loginResult = await _userService.LoginAsync(userName, password);
                    if (loginResult.Succeeded)
                    {
                        if (loginResult.IsLockedOut || loginResult.IsNotAllowed)
                        {
                            return (false, "Tài khoản đã bị khóa");
                        }
                        var claims = new List<Claim> { new Claim(ClaimTypes.Name, userName) };
                        if (userName.ToUpper() == "ADMIN")
                        {
                            claims.Add(new Claim(CustomClaimTypes.Permission, PermissionKey.ADMIN));
                        }
                        else
                        {
                            var claimDatas = await _userService.GetAllClaimsAsync(userName);
                            foreach (var roleName in claimDatas)
                            {
                                claims.Add(new Claim(roleName.Type, roleName.Value));
                            }
                        }
                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        var authProperties = new AuthenticationProperties
                        {
                            AllowRefresh = true,
                            ExpiresUtc = DateTimeOffset.UtcNow.AddDays(30),
                            IsPersistent = isRemember,
                        };

                        await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity),
                            authProperties);
                        HttpContext.Response.Cookies.Append(GlobalVariants.ACCESSTOKEN, loginResult.AuthenToken, new CookieOptions
                        {
                            Expires = DateTimeOffset.UtcNow.AddDays(30),
                            HttpOnly = true,
                            SameSite = SameSiteMode.Strict,
                            Path = "/",
                            Secure = true
                        });

                        return (true, "Xác thực thành công");

                    }
                    else
                    {
                        return (false, "Tài khoản hoặc mật khẩu không hợp lệ");
                    }
                }
                return (false, "Tài khoản hoặc mật khẩu không được bỏ trống.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return (false, "Xác thực tài khoản thất bại.");
            }
        }

    }
}

