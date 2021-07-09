using System;
using System.Text;
using System.Collections.Generic;


namespace Course.Web.Share.Domain {
    
    public class Lessons {
        public virtual string Id { get; set; }
        public virtual string IdKhoaHoc { get; set; }
        public virtual string NoiDung { get; set; }
        public virtual string FileNoiDung { get; set; }
        public virtual string MaBaiHoc { get; set; }
    }
}
