using NHibernate;
using NHibernate.AspNetCore.Identity;
using NHibernate.Mapping.ByCode.Conformist;
using Course.Web.Share.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Course.Web.Share.Map
{
    public class AppRoleMap : JoinedSubclassMapping<AppRole>
    {
        public AppRoleMap()
        {
            ExplicitDeclarationsHolder
                .AddAsRootEntity(typeof(IdentityRole));
            Extends(typeof(IdentityRole));
            Schema("dbo");
            Table("AppRoles");
            Key(k => k.Column("Id"));
            Property(
                p => p.Description,
                maping => {
                    maping.Column("Description");
                    maping.Type(NHibernateUtil.String);
                    maping.Length(256);
                }
            );
        }

    }
}
