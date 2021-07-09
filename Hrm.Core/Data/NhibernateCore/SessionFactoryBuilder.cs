using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cache;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Course.Core.Data.NhibernateCore
{
    public class SessionFactoryBuilder
    {
        public static ISessionFactory BuildSessionFactory(string connectionString, string assemblyName, bool create = false, bool update = false)
        {
            return Fluently.Configure()
            .Database(MsSqlConfiguration.MsSql2012.ConnectionString(connectionString))
            .Cache(c => c.UseQueryCache().ProviderClass<NHibernate.Caches.CoreMemoryCache.CoreMemoryCacheProvider>().UseSecondLevelCache())
            .Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.Load(assemblyName)))
            .ExposeConfiguration(cfg => cfg.SetProperty("adonet.batch_size", "50"))
            .BuildConfiguration()
            .BuildSessionFactory();
        }

        public static ISessionFactory BuildSessionFactory(NHibernate.Cfg.Configuration config, string assemblyName, bool create = false, bool update = false)
        {
            return Fluently.Configure(config)
            .Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.Load(assemblyName)))
            .BuildConfiguration()
            .BuildSessionFactory();
        }


        /// <summary>
        /// Build the schema of the database.
        /// </summary>
        /// <param name="config">Configuration.</param>
        private static void BuildSchema(Configuration config, bool create = false, bool update = false)
        {
            if (create)
            {
                new SchemaExport(config).Create(false, true);
            }
            else
            {
                new SchemaUpdate(config).Execute(false, update);
            }
        }
    }
}
