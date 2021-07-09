using System; 
using System.Collections.Generic; 
using System.Text; 
using FluentNHibernate.Mapping;
using Course.Web.Share.Domain; 

namespace Course.Web.Share.Map
{
    
    public class LessonsMap : ClassMap<Lessons> {
        
        public LessonsMap() {
			Table("Lessons");
			LazyLoad();
			Id(x => x.Id).GeneratedBy.Assigned().Column("Id");
			Map(x => x.IdKhoaHoc).Column("IdKhoaHoc").Not.Nullable();
			Map(x => x.NoiDung).Column("NoiDung");
			Map(x => x.FileNoiDung).Column("FileNoiDung");
			Map(x => x.MaBaiHoc).Column("MaBaiHoc");
        }
    }
}
