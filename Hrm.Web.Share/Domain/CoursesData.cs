using Course.Core.Data;
using System; 
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text; 


namespace Course.Web.Share.Domain {
    
    
    [DataContract(Name="CoursesData" , Namespace="")]
    public class CoursesData : ISelectItem
    {
        
        [DataMember(Order=1)]
        public virtual String Id {
            get;
            set;
        }
        
        [DataMember(Order=2)]
        public virtual String TenKhoaHoc {
            get;
            set;
        }
        
        [DataMember(Order=3)]
        public virtual String MaKhoaHoc {
            get;
            set;
        }
        
        [DataMember(Order=4)]
        public virtual String ThoiLuong {
            get;
            set;
        }
        
        [DataMember(Order=5)]
        public virtual DateTime? TuNgay {
            get;
            set;
        }
        
        [DataMember(Order=6)]
        public virtual DateTime? DenNgay {
            get;
            set;
        }        
        [DataMember(Order=7)]
        public virtual Boolean Active {
            get;
            set;
        }

        public string GetCustomDisplay()
        {
            return string.Format("{0} - {1}", MaKhoaHoc, TenKhoaHoc);
        }

        public string GetDisplay()
        {
            return TenKhoaHoc;
        }

        public string GetKey()
        {
            return Id;
        }

        public void SetKey(string key)
        {
            Id = key;
        }
    }
}
