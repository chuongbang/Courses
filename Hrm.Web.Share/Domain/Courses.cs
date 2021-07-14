using System;
using System.Text;
using System.Collections.Generic;


namespace Course.Web.Share.Domain {
    
    public class Courses 
    {
        public virtual string Id { get; set; }
        public virtual string TenKhoaHoc { get; set; }
        public virtual string MaKhoaHoc { get; set; }
        public virtual string ThoiLuong { get; set; }
        public virtual DateTime? TuNgay { get; set; }
        public virtual DateTime? DenNgay { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual decimal? HocPhi { get; set; }
        public virtual string GiaoVien { get; set; }
    }
}
