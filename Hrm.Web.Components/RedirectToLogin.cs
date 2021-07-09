using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Hosting;

namespace Course.Web.Components
{
    public class RedirectToLogin : LayoutComponentBase
    {
        [Inject] NavigationManager NavigationManager { get; set; }
        [Inject] IHostEnvironment Env { get; set; }

        protected override void OnInitialized()
        {
            string uri = NavigationManager.BaseUri + "dang-nhap";
            if (!Env.IsDevelopment())
            {
                if (uri.StartsWith("http:"))
                {
                    uri = "https" + uri.Substring(4);
                }
            }
            NavigationManager.NavigateTo(uri);
        }
    }
}
