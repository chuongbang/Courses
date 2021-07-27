using System;
using System.Text;
using System.Collections.Generic;


namespace Course.Web.Share.Domain {
    
    public class UserCourses 
    {
        public virtual string Id { get; set; }
        public virtual string UserId { get; set; }
        public virtual string KhoaHocId { get; set; }
        public virtual DateTime? TuNgay { get; set; }
        public virtual DateTime? DenNgay { get; set; }
        public virtual bool IsTrial { get; set; }

    }
}
