using System;
using System.Text;
using System.Collections.Generic;


namespace Course.Web.Share.Domain {

    public class AuditLog
    {
        public virtual string Id { get; set; }
        public virtual string Username { get; set; }
        public virtual string Action { get; set; }
        public virtual DateTime Timestamp { get; set; }
        public virtual string TableName { get; set; }
        public virtual string RecordId { get; set; }
        public virtual string Title { get; set; }
        public virtual IList<AuditLogDetail> AuditLogDetails { get; set; }
    }

    public class AuditLogDetail
    {
        public virtual string Id { get; set; }
        public virtual AuditLog AuditLog { get; set; }
        public virtual string FieldName { get; set; }
        public virtual string OldValue { get; set; }
        public virtual string NewValue { get; set; }
    }
}
