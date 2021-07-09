using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Course.Web.Share.Map;

namespace Course.Web.Share.Ultils
{
    public static class AutoMapperExtentions
    {
        public static void AddAutoMapperConfig(this IServiceCollection services)
        {
            #region Add automapper
            var assembly = Assembly.GetAssembly(typeof(AutoMapperExtentions));
            services.AddAutoMapper(assembly);
            #endregion
            services.AddAutoMapper(typeof(AppMappingProfile));
        }
    }
}
