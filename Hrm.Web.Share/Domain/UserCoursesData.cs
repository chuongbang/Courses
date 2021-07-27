using Course.Core.Data;
using System; 
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text; 


namespace Course.Web.Share.Domain {
    
    
    [DataContract(Name= "UserCoursesData", Namespace="")]
    public class UserCoursesData
    {
        
        [DataMember(Order=1)]
        public virtual String Id {
            get;
            set;
        }
        
        [DataMember(Order=2)]
        public virtual String UserId {
            get;
            set;
        }
        
        [DataMember(Order=3)]
        public virtual String KhoaHocId {
            get;
            set;
        }       
        [DataMember(Order=4)]
        public virtual DateTime? TuNgay {
            get;
            set;
        }      
        [DataMember(Order=5)]
        public virtual DateTime? DenNgay {
            get;
            set;
        }        
        [DataMember(Order=6)]
        public virtual Boolean IsTrial
        {
            get;
            set;
        }
        


    }
}
