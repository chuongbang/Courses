using System; 
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text; 
using Course.Web.Share.Domain;


namespace Course.Web.Share.Domain {
    
    
    [DataContract(Name="AppRoleData" , Namespace="")]
    public class AppRoleData {

        [DataMember(Order = 1)]
        public virtual String Id
        {
            get;
            set;
        }

        [DataMember(Order = 2)]
        public virtual String Name
        {
            get;
            set;
        }

        [DataMember(Order = 3)]
        public virtual String NormalizedName
        {
            get;
            set;
        }

        [DataMember(Order = 4)]
        public virtual String ConcurrencyStamp
        {
            get;
            set;
        }

        [DataMember(Order=5)]
        public virtual String Description {
            get;
            set;
        }
    }
}
