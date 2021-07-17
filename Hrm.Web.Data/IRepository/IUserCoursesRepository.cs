using System.Collections.Generic;
using System.Threading.Tasks;
using Course.Core.Patterns.Repository;
using Course.Web.Share.Domain;

namespace Course.Web.Data.IRepository
{
    public interface IUserCoursesRepository : IRepository<UserCourses>
    {
        Task<(List<UserCourses>, int)> GetByIdAsync(string id, string keyword, int pageIndex, int pageSize);
        Task<(List<Courses>, int)> GetPageByIdAsync(string id, string keyword, int pageIndex, int pageSize);

    }
}
