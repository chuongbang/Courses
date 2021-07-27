using System;
using System.Text;
using System.Collections.Generic;


namespace Course.Web.Share.Domain {
    
    public class Lessons {
        public virtual string Id { get; set; }
        public virtual string KhoaHocId { get; set; }
        public virtual string NoiDung { get; set; }
        public virtual string FileNoiDung { get; set; }
        public virtual string MaBaiHoc { get; set; }
        public virtual bool IsTrial { get; set; }
        public virtual string TenBaiHoc { get; set; }
        public virtual string TypeContent { get; set; }
    }
}
