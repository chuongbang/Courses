using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Criterion;
using System.Xml.Linq;
using ProtoBuf.Grpc;
using System.ServiceModel;
using Course.Web.Share.Domain;
using System.Security.Claims;
using System.Runtime.InteropServices.ComTypes;
using System.ComponentModel.DataAnnotations;
using Course.Core.Extentions;

namespace Course.Web.Share.IServices
{
    [ServiceContract(Name = "Course.AuditLogService")]
    public interface IAuditLogService
    {
        Task<ListAuditLogResult> GetPageWithFilterAsync(AuditLogSearch model);

    }

    [DataContract]
    public class ListAuditLogResult
    {
        [DataMember(Order = 1)]
        public List<AuditLogData> Data { get; set; }
        [DataMember(Order = 2)]
        public int Total { get; set; }

    }

    [DataContract]
    public class AuditLogSearch
    {
        [DataMember(Order = 1)]
        public string Id { get; set; }
        [DataMember(Order = 2)]
        public Page Page { get; set; }

        [DataMember(Order = 3)]
        public virtual string Username { get; set; }

        [DataMember(Order = 4)]
        public virtual string Action { get; set; }

        [DataMember(Order = 5)]
        public virtual string TableName { get; set; }

        [DataMember(Order = 6)]
        public virtual string Title { get; set; }

        [DataMember(Order = 7)]
        public virtual DateTime? StartDate { get; set; }
        [DataMember(Order = 8)]
        public virtual DateTime? EndDate { get; set; }

        public IQueryable<AuditLog> CreateFilter(IQueryable<AuditLog> filter)
        {
            int total = 0;
            if (Id.IsNotNullOrEmpty())
            {
                filter = filter.Where(c => c.Id == Id);
            }
            if (Username.IsNotNullOrEmpty())
            {
                filter = filter.Where(c => c.Username == Username);
            }
            if (Action.IsNotNullOrEmpty())
            {
                filter = filter.Where(c => c.Action == Action);
            }
            if (TableName.IsNotNullOrEmpty())
            {
                filter = filter.Where(c => c.TableName == TableName);
            }
            if (Title.IsNotNullOrEmpty())
            {
                filter = filter.Where(c => c.Title.Contains(Title));
            }
            if (StartDate.HasValue)
            {
                filter = filter.Where(c => c.Timestamp >= StartDate.Value.Date);
            }
            if (EndDate.HasValue)
            {
                filter = filter.Where(c => c.Timestamp < EndDate.Value.Date.AddDays(1));
            }
            return filter;
        }
    }

    [DataContract]
    public class DeleteAuditLog
    {
        [DataMember(Order = 1)]
        public string Id { get; set; }
    }

}
