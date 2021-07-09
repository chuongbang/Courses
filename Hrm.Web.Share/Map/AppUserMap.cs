using NHibernate;
using NHibernate.AspNetCore.Identity;
using NHibernate.Mapping;
using NHibernate.Mapping.ByCode.Conformist;
using Course.Web.Share.Domain;

namespace Course.Web.Share.Map
{
    public class AppUserMap : JoinedSubclassMapping<AppUser>
    {

        public AppUserMap()
        {
            Extends(typeof(IdentityUser));
            ExplicitDeclarationsHolder
                .AddAsRootEntity(typeof(IdentityUser));

            Table("AppUsers");
            Schema("dbo");
            Key(x => x.Column("Id"));
            Property(
                e => e.CreateTime,
                mapping =>
                {
                    mapping.Column("CreateTime");
                    mapping.Type(NHibernateUtil.DateTime);
                    mapping.Update(false);
                    mapping.Generated(NHibernate.Mapping.ByCode.PropertyGeneration.Insert);
                    mapping.NotNullable(true);
                }
            );
            Property(
                e => e.LastLogin,
                mapping =>
                {
                    mapping.Column("LastLogin");
                    mapping.Type(NHibernateUtil.DateTime);
                    mapping.NotNullable(false);
                }
            );
            Property(
                e => e.LoginCount,
                mapping =>
                {
                    mapping.Column("LoginCount");
                    mapping.Type(NHibernateUtil.Int32);
                    mapping.NotNullable(true);
                }
            );
            Property(
                e => e.DateOfBirth,
                mapping =>
                {
                    mapping.Column("DateOfBirth");
                    mapping.Type(NHibernateUtil.DateTime);
                    mapping.NotNullable(false);
                }
            );
            Property(
                e => e.FullName,
                mapping =>
                {
                    mapping.Column("FullName");
                    mapping.Type(NHibernateUtil.String);
                }
            );
            Property(
                e => e.JobTitle,
                mapping =>
                {
                    mapping.Column("JobTitle");
                    mapping.Type(NHibernateUtil.String);
                }
            );
            Property(
                e => e.IsActive,
                mapping =>
                {
                    mapping.Column("IsActive");
                    mapping.Type(NHibernateUtil.Boolean);
                    mapping.NotNullable(true);
                }
            );
            Property(
                e => e.KhoaHocIds,
                mapping =>
                {
                    mapping.Column("KhoaHocIds");
                    mapping.Type(NHibernateUtil.String);
                }
            );
            Property(
                e => e.ExpiredDate,
                mapping =>
                {
                    mapping.Column("ExpiredDate");
                    mapping.Type(NHibernateUtil.DateTime);
                }
            );

        }

    }
}
