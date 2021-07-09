using System.Collections.Generic;
using System.Threading.Tasks;
using Course.Core.Patterns.Repository;
using Course.Web.Share.Domain;

namespace Course.Web.Data.IRepository
{
    public interface ICoursesRepository : IRepository<Courses>
    {
        Task<(List<Courses>, int)> GetByIdsAsync(IEnumerable<string> ids, string keyword, int pageIndex, int pageSize);

        //Task<bool> DeleteListAsync(IEnumerable<Courses> list);

    }
}
