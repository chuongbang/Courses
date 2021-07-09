using System; 
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text; 


namespace Course.Web.Share.Domain {
    
    
    [DataContract(Name="LessonsData" , Namespace="")]
    public class LessonsData {
        
        [DataMember(Order=1)]
        public virtual String Id {
            get;
            set;
        }
        
        [DataMember(Order=2)]
        public virtual String IdKhoaHoc {
            get;
            set;
        }
        
        [DataMember(Order=3)]
        public virtual String NoiDung {
            get;
            set;
        }
        
        [DataMember(Order=4)]
        public virtual String FileNoiDung {
            get;
            set;
        }
        
        [DataMember(Order=5)]
        public virtual String MaBaiHoc {
            get;
            set;
        }
    }
}