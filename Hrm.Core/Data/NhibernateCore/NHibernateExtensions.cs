using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.AspNetCore.Identity;
using NHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Cfg;

namespace Course.Core.Data.NhibernateCore
{
    public static class NHibernateExtensions
    {
        public static IServiceCollection AddNHibernateForMsSql(this IServiceCollection services, string connectionString, string assemblyName)
        {
            Configuration cfg = Fluently.Configure()
            .Database(MsSqlConfiguration.MsSql2012.ConnectionString(connectionString)).BuildConfiguration()
            .AddIdentityMappingsForMsSql();
            services.AddSingleton(x => SessionFactoryBuilder.BuildSessionFactory(cfg, assemblyName));
            services.AddScoped(factory =>
               factory
                    .GetServices<ISessionFactory>()
                    .First()
                    .OpenSession());

            return services;
        }

        public static IServiceCollection AddNHibernateForMsSql(this IServiceCollection services, Configuration cfg, string assemblyName)
        {
            cfg.AddIdentityMappingsForMsSql();
            services.AddSingleton(x => SessionFactoryBuilder.BuildSessionFactory(cfg, assemblyName));
            services.AddScoped(factory =>
               factory
                    .GetServices<ISessionFactory>()
                    .First()
                    .OpenSession());

            return services;
        }

        public static IServiceCollection AddNHibernate(this IServiceCollection services, Configuration cfg, string assemblyName)
        {
            services.AddSingleton(x => SessionFactoryBuilder.BuildSessionFactory(cfg, assemblyName));
            services.AddScoped(factory =>
               factory
                    .GetServices<ISessionFactory>()
                    .First()
                    .OpenSession());

            return services;
        }

        public static IServiceCollection AddNhibernateIdentityCore<TUser, TRole>(this IServiceCollection services)
            where TUser : class
            where TRole : class
        {
            services.AddIdentity<TUser, TRole>()
                .AddHibernateStores();
            return services;
        }

        public static IServiceCollection AddNhibernateIdentityCore<TUser, TRole>(this IServiceCollection services, Action<IdentityOptions> setupAction)
             where TUser : class
            where TRole : class
        {
            services.AddIdentity<TUser, TRole>(setupAction)
                .AddHibernateStores();
            return services;
        }
    }
}
