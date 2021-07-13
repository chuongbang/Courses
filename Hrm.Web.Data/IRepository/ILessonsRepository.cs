using System.Collections.Generic;
using System.Threading.Tasks;
using Course.Core.Patterns.Repository;
using Course.Web.Share.Domain;

namespace Course.Web.Data.IRepository
{
    public interface ILessonsRepository : IRepository<Lessons>
    {
        Task<(List<Lessons>, int)> GetByIdsAsync(IEnumerable<string> ids, string keyword, int pageIndex, int pageSize);
        Task<List<Lessons>> GetAllActiveAsync();

        //Task<bool> DeleteListAsync(IEnumerable<Courses> list);

    }
}
