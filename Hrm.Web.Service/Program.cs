using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Course.Web.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            HibernatingRhinos.Profiler.Appender.NHibernate.NHibernateProfiler.Initialize();
            CreateHostBuilder(args).Build().Run();
        }

        // Additional configuration is required to successfully run gRPC on macOS.
        // For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
        public static IWebHostBuilder CreateHostBuilder(string[] args)
        {
            IWebHostEnvironment hostingEnvironment = null;
            var webHost = WebHost.CreateDefaultBuilder(args)
            .UseContentRoot(Directory.GetCurrentDirectory())
            .ConfigureServices(
                services =>
                {
                    hostingEnvironment = services
                        .Where(x => x.ServiceType == typeof(IWebHostEnvironment))
                        .Select(x => (IWebHostEnvironment)x.ImplementationInstance)
                        .First();

                })
            .UseKestrel(options =>
            {
                var config = new ConfigurationBuilder()
                    .AddJsonFile($"appsettings.{hostingEnvironment.EnvironmentName}.json", optional: false)
                    .Build();
                options.ListenAnyIP(config.GetValue<int>("Host:Port"));
                options.ListenLocalhost(config.GetValue<int>("Host:Port"));
            })
            .UseStartup<Startup>();

            return webHost;
        }
    }
}
