using System; 
using System.Collections.Generic; 
using System.Text; 
using FluentNHibernate.Mapping;
using Course.Web.Share.Domain;

namespace Course.Web.Share.Map
{

    public class AuditLogMap : ClassMap<AuditLog>
    {
        public AuditLogMap()
        {
            Table("AuditLogs");
            LazyLoad();
            Id(x => x.Id);
            Map(x => x.Username);
            Map(x => x.Action);
            Map(x => x.Timestamp);
            Map(x => x.TableName);
            Map(x => x.RecordId);
            Map(x => x.Title);
            HasMany(x => x.AuditLogDetails);
        }
    }

    public class AuditLogDetailMap : ClassMap<AuditLogDetail>
    {
        public AuditLogDetailMap()
        {
            Table("AuditLogDetails");
            LazyLoad();
            Id(x => x.Id);
            References(x => x.AuditLog).Column("AuditLogId");
            Map(x => x.FieldName);
            Map(x => x.OldValue);
            Map(x => x.NewValue);
        }
    }
}
