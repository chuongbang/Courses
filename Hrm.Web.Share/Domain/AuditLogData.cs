using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Course.Web.Share.Domain;


namespace Course.Web.Share.Domain
{


    [DataContract(Name = "AuditLogData", Namespace = "")]
    public class AuditLogData
    {

        [DataMember(Order = 1)]
        public virtual string Id
        {
            get;
            set;
        }

        [DataMember(Order = 2)]
        public virtual String Username
        {
            get;
            set;
        }

        [DataMember(Order = 3)]
        public virtual String Action
        {
            get;
            set;
        }

        [DataMember(Order = 4)]
        public virtual DateTime Timestamp
        {
            get;
            set;
        }

        [DataMember(Order = 5)]
        public virtual String TableName
        {
            get;
            set;
        }

        [DataMember(Order = 6)]
        public virtual String RecordId
        {
            get;
            set;
        }

        [DataMember(Order = 7)]
        public virtual String Title
        {
            get;
            set;
        }

        [DataMember(Order = 8)]
        public virtual List<AuditLogDetailData> AuditLogDetails { get; set; }
    }
}
