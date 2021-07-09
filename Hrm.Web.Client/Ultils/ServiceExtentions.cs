using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Course.Web.Client.Ultils
{
    public static class ServiceExtentions
    {
        public static void AddServices(this IServiceCollection services)
        {
            #region Đăng ký các custom service
            // Tự động đăng ký các service
            var assembly = Assembly.GetExecutingAssembly();
            var classes = assembly.ExportedTypes
               .Where(a => a.FullName.EndsWith("AdapterService"));
            foreach (Type implement in classes)
            {
                services.AddScoped(implement);
            }
            #endregion
        }

    }
}
