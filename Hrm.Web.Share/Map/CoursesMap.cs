using System; 
using System.Collections.Generic; 
using System.Text; 
using FluentNHibernate.Mapping;
using Course.Web.Share.Domain; 

namespace Course.Web.Share.Map
{
    
    public class CoursesMap : ClassMap<Courses> {
        
        public CoursesMap() {
			Table("Courses");
			LazyLoad();
			Id(x => x.Id).GeneratedBy.Assigned().Column("Id");
			Map(x => x.TenKhoaHoc).Column("TenKhoaHoc").Not.Nullable();
			Map(x => x.MaKhoaHoc).Column("MaKhoaHoc");
			Map(x => x.ThoiLuong).Column("ThoiLuong");
			Map(x => x.TuNgay).Column("TuNgay");
			Map(x => x.DenNgay).Column("DenNgay");
			Map(x => x.Active).Column("Active");
        }
    }
}
