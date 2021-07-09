using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Course.Core.Extentions;

namespace Course.Web.Client
{
    [AllowAnonymous]
    public class LogoutModel : PageModel
    {
        private readonly ILogger<LogoutModel> _Logger;
        public LogoutModel(ILogger<LogoutModel> logger)
        {
            _Logger = logger;
        }
        public async Task<IActionResult> OnGetAsync(string sub = "")
        {
            // Clear the existing external cookie
            try
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                _Logger.LogInformation(string.Format("{0} Logged out", HttpContext.User.Identity.Name));
            }
            catch (Exception)
            {
            }
            return LocalRedirect(Url.Content("~/dang-nhap"));
        }
    }
}
