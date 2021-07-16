using System.Collections.Generic;
using System.Threading.Tasks;
using Course.Core.Patterns.Repository;
using Course.Web.Share.Domain;
using Course.Web.Share.IServices;

namespace Course.Web.Data.IRepository
{
    public interface ICoursesRepository : IRepository<Courses>
    {
        Task<(List<Courses>, int)> GetByIdsAsync(IEnumerable<string> ids, string keyword, int pageIndex, int pageSize);
        Task<List<Courses>> GetAllActiveAsync();
        Task<(List<Courses>, List<Lessons>, int)> GetCoursesActiveWithLessonsAsync(CoursesSearch cs);

        //Task<bool> DeleteListAsync(IEnumerable<Courses> list);

    }
}
