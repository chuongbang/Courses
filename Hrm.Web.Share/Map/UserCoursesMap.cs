using System; 
using System.Collections.Generic; 
using System.Text; 
using FluentNHibernate.Mapping;
using Course.Web.Share.Domain; 

namespace Course.Web.Share.Map
{
    
    public class UserCoursesMap : ClassMap<UserCourses> {
        
        public UserCoursesMap() {
			Table("UserCourses");
			LazyLoad();
			Id(x => x.Id).GeneratedBy.Assigned().Column("Id");
			Map(x => x.UserId).Column("UserId").Not.Nullable();
			Map(x => x.KhoaHocId).Column("KhoaHocId");
			Map(x => x.TuNgay).Column("TuNgay");
			Map(x => x.DenNgay).Column("DenNgay");
        }
    }
}
