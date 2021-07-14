using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Cfg;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NHibernate;
using ProtoBuf.Grpc.Server;
using Course.Core.Data.NhibernateCore;
using Course.Core.Ultis;
using Course.Web.Data.Helper;
using Course.Web.Service.Services;
using Course.Web.Service.Ultils;
using Course.Web.Share.Domain;
using Course.Web.Share.Map;
using Course.Web.Share;
using Course.Web.Share.Ultils;
using NHibernate.Event;
using NHibernate.Cfg;
using NHibernate.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Course.Web.Service.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Course.Web.Service
{
    public class Startup
    {
        public IConfigurationRoot Configuration
        {
            get;
            set;
        }
        public IWebHostEnvironment Env { get; set; }
        public static string ConnectionString
        {
            get;
            private set;
        }
        public Startup(IWebHostEnvironment env)
        {
            if (env.IsProduction())
            {
                Configuration = new ConfigurationBuilder().SetBasePath(env.ContentRootPath).AddJsonFile("appSettings.Production.json").Build();
            }
            else if (env.IsDevelopment())
            {
                Configuration = new ConfigurationBuilder().SetBasePath(env.ContentRootPath).AddJsonFile("appsettings.Development.json").Build();
            }
            else
            {
                Configuration = new ConfigurationBuilder().SetBasePath(env.ContentRootPath).AddJsonFile("appSettings.json").Build();
            }
            Env = env;
            ConnectionString = EncryptionHelper.Decrypt(Configuration["ConnectionStrings:Connection"], ".gUju7KkmNPaF&vh+RmM_@yNyTx-LrwyA63_`yK4Wsp(}[AT@/Y9'T%^~;*su7/pevpZmA$d`K/<NPwa'Ns)EY<@95Tts`-yBJ>?9Eu=Sdn=JYEkQe<4J`&s-vV47");
        }

        public static string GetConnectionString()
        {
            return ConnectionString;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddCodeFirstGrpc(options =>
            {
                options.EnableDetailedErrors = true;
            });

            services.AddRepository();
            services.AddGrpcService();
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddHealthChecks();
            services.AddAutoMapperConfig();

            services.AddScoped<AuditEventListener>(serviceProvider =>
            {
                var options = serviceProvider.GetService<IHttpContextAccessor>();
                var bar = new AuditEventListener(options);
                return bar;
            });
            //var auditEventListener = new AuditEventListener(services.BuildServiceProvider().GetService<IHttpContextAccessor>());
            var auditEventListener = ActivatorUtilities.GetServiceOrCreateInstance<AuditEventListener>(services.BuildServiceProvider());
            #region Config Nhibernate and Repositories
            var cfg = Fluently.Configure()
            .Database(MsSqlConfiguration.MsSql2012.ConnectionString(GetConnectionString()))
            .ExposeConfiguration(config =>
            {
                config.EventListeners.PostInsertEventListeners =
                    new IPostInsertEventListener[]
                        { auditEventListener };
                config.EventListeners.PostUpdateEventListeners =
                    new IPostUpdateEventListener[]
                        { auditEventListener };
                config.EventListeners.PostDeleteEventListeners =
                    new IPostDeleteEventListener[]
                        { auditEventListener };
            })
            .BuildConfiguration();
            if (Env.IsDevelopment())
            {
                cfg.DataBaseIntegration(c =>
                {
                    c.LogSqlInConsole = true;
                    c.LogFormattedSql = true;
                });
            }
            cfg.AddIdentityMappingsForMsSql();
            var modelMapper = new NHibernate.Mapping.ByCode.ModelMapper();
            modelMapper.AddMapping<AppUserMap>();
            modelMapper.AddMapping<AppRoleMap>();
            var mappings = modelMapper.CompileMappingForAllExplicitlyAddedEntities();
            cfg.AddMapping(mappings);
            services.AddNHibernate(cfg, typeof(AppUser).Assembly.GetName().Name);
            services.AddNhibernateIdentityCore<AppUser, AppRole>(options =>
            {
                options.User.RequireUniqueEmail = false;
            });

            #endregion

            #region Authentication
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, (o) =>
            {
                o.TokenValidationParameters = new TokenValidationParameters()
                {
                    IssuerSigningKey = TokenAuthOption.Key,
                    ValidAudience = TokenAuthOption.Audience,
                    ValidIssuer = TokenAuthOption.Issuer,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                };
            });
            services.AddAuthorization(auth =>
            {
                auth.AddPolicy(JwtBearerDefaults.AuthenticationScheme, new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build());
            });
            #endregion
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            ConnectionString = Configuration["ConnectionStrings:Connection"];

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health");
                endpoints.MapGrpcService<AppUserService>();
                endpoints.MapGrpcService<AppRoleService>();
                endpoints.MapGrpcService<AuditLogService>();
                endpoints.MapGrpcService<CoursesService>();
                endpoints.MapGrpcService<UserCoursesService>();
                endpoints.MapGrpcService<LessonsService>();

                endpoints.MapGet("/", async context =>
                {
                    context.Response.ContentType = "text/plain; charset=utf-8";
                    await context.Response.WriteAsync("Hệ thống Course.");
                });

            });
        }
    }
}
