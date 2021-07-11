using System.Collections.Generic;
using System.Threading.Tasks;
using Course.Core.Patterns.Repository;
using Course.Web.Share.Domain;

namespace Course.Web.Data.IRepository
{
    public interface IUserCoursesRepository : IRepository<UserCourses>
    {
        Task<List<UserCourses>> GetByIdAsync(string id);


    }
}
