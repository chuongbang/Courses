using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace Course.Web.Data.Helper
{
    public static class BuildRepositories
    {
        public static void AddRepository(this IServiceCollection services)
        {
            #region Đăng ký các repository
            var assembly = Assembly.GetAssembly(typeof(BuildRepositories));
            var classes = assembly.ExportedTypes
               .Where(a => !a.Name.StartsWith("I") && a.Name.EndsWith("Repository"));
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
