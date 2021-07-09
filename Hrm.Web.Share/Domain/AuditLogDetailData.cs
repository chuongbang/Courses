using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Course.Web.Share.Domain;


namespace Course.Web.Share.Domain
{


    [DataContract(Name = "AuditLogDetailData", Namespace = "")]
    public class AuditLogDetailData
    {

        [DataMember(Order = 1)]
        public virtual string Id
        {
            get;
            set;
        }

        [DataMember(Order = 2)]
        public virtual string AuditLogId
        {
            get;
            set;
        }

        [DataMember(Order = 3)]
        public virtual String FieldName
        {
            get;
            set;
        }

        [DataMember(Order = 4)]
        public virtual String OldValue
        {
            get;
            set;
        }

        [DataMember(Order = 5)]
        public virtual String NewValue
        {
            get;
            set;
        }
    }
}
