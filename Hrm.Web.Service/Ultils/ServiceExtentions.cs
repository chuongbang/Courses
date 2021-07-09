using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Course.Web.Service.Ultils
{
    public static class ServiceExtentions
    {
        public static void AddGrpcService(this IServiceCollection services)
        {
            #region Đăng ký các custom service
            // Tự động đăng ký các service
            var assembly = Assembly.GetExecutingAssembly();
            var classes = assembly.ExportedTypes
               .Where(a => a.FullName.EndsWith("Service"));
            foreach (Type implement in classes)
            {
                foreach (var @interface in implement.GetInterfaces())
                {
                    services.AddScoped(@interface, implement);
                }
            }
            #endregion
        }
    }
}
