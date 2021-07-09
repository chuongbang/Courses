using Course.Core.AuditLog;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Web.Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            string contentDirectory = Directory.GetCurrentDirectory();
            IHostEnvironment hostingEnvironment = null;
            var webHost = Host.CreateDefaultBuilder(args)
                .UseContentRoot(contentDirectory)
                .ConfigureServices(
                services =>
                {
                    hostingEnvironment = services
                        .Where(x => x.ServiceType == typeof(IHostEnvironment))
                        .Select(x => (IHostEnvironment)x.ImplementationInstance)
                        .First();

                })
                .ConfigureLogging(logging =>
                {
                    logging.AddFile(options =>
                    {
                        IConfiguration config;
                        if (hostingEnvironment.IsDevelopment())
                        {
                            config = new ConfigurationBuilder()
                                .AddJsonFile($"appsettings.{hostingEnvironment.EnvironmentName}.json", optional: false)
                                .Build();
                            options.LogDirectory = Path.Combine(contentDirectory, "bin", "ProgramLogs");
                        }
                        else
                        {
                            config = new ConfigurationBuilder()
                                .AddJsonFile($"appsettings.json", optional: false)
                                .Build();
                            options.LogDirectory = Path.Combine(contentDirectory, "ProgramLogs");
                        }
                        options.Periodicity = PeriodicityOptions.Daily;



                    });
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
            return webHost;
        }
    }
}
