using Course.Web.Client.Ultils;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using System.Threading.Tasks;
using Course.Web.Share.Ultils;
using Course.Web.Client.Service;
using Microsoft.AspNetCore.Identity;
using Course.Web.Share.Domain;
using Course.Web.Share;

namespace Course.Web.Client
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
            GlobalVariants.InitFileVersion();
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Env { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddResponseCompression();
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.Strict;
            });
            services.AddAntiforgery(options =>
            {
                options.Cookie.Name = ".FW.Antiforgery";
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
            });
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.Cookie.Name = ".FW.Cookies";
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                    options.LoginPath = new PathString("/dang-nhap");
                    //options.ReturnUrlParameter = "RequestPath";
                    options.SlidingExpiration = true;
                    options.Cookie.HttpOnly = true;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                    options.Events = new CookieAuthenticationEvents
                    {
                        OnRedirectToLogin = ctx =>
                        {
                            string uri = "http://" + ctx.Request.Host.Host + "/dang-nhap";
                            //if (!Env.IsDevelopment())
                            //{
                            //    if (uri.StartsWith("http:"))
                            //    {
                            //        uri = "https" + uri.Substring(4);
                            //    }
                            //}
                            ctx.Response.Redirect(uri);

                            return Task.CompletedTask;
                        }
                    };
                });
            services.AddAuthorization();
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddAntDesign();
            services.AddAutoMapperConfig();
            services.AddDataService();
            services.AddSingleton(c => new HostService { Url = Configuration["ServiceHost"] });
            services.AddScoped<TokenProvider>();
            services.AddHttpContextAccessor();
            services.AddScoped<PermissionClaim>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            //Log all errors in the application
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(context =>
                {
                    var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
                    var exception = errorFeature.Error;
                    logger.LogError(String.Format("\nMessage of error: {0}\nStacktrace of error: {1}", exception.Message, exception.StackTrace.ToString()));
                    return System.Threading.Tasks.Task.CompletedTask;
                });
            });

            var supportedCultures = new[]
            {
                new CultureInfo("vi-VN"),
            };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("vi-VN"),
                // Formatting numbers, dates, etc.
                SupportedCultures = supportedCultures,
                // UI strings that we have localized.
                SupportedUICultures = supportedCultures
            });

            app.UseStaticFiles();

            //app.UseCookiePolicy();
            app.UseAuthentication();

            app.UseRouting();
            app.UseAuthorization();

            app.UseCsp(opts => opts
                 .BlockAllMixedContent()
                 .StyleSources(s => s.Self())
                 .StyleSources(s => s.UnsafeInline())
                 .StyleSources(s => s.CustomSources("stackpath.bootstrapcdn.com", "fonts.googleapis.com"))
                 .FontSources(s => s.Self())
                 .FontSources(s => s.CustomSources("fonts.gstatic.com", "stackpath.bootstrapcdn.com"))
                 .FormActions(s => s.Self())
                 .FrameAncestors(s => s.Self())
                 .ImageSources(s => s.Self())
                 .ScriptSources(s => s.Self())
                 .ScriptSources(s => s.UnsafeEval())
                 .ScriptSources(s => s.CustomSources("blob:"))
                 .ObjectSources(s => s.None())
                 );

            app.UseXXssProtection(options => options.EnabledWithBlockMode());
            app.UseXContentTypeOptions();
            app.UseXDownloadOptions();
            app.UseXfo(options => options.Deny());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
