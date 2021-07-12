using NHibernate;
using NHibernate.Linq;
using Course.Core.Extentions;
using Course.Core.Patterns.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Course.Web.Data.IRepository;
using Course.Web.Share.Domain;

namespace Course.Web.Data.Repository
{
    public class UserCoursesRepository : Repository<UserCourses>, IUserCoursesRepository
    {
        IQueryable<Courses> _courseQr;
        public UserCoursesRepository(ISession session)
            : base(session)
        {
            _courseQr = session.Query<Courses>();
        }


        public async Task<List<UserCourses>> GetByIdAsync(string id)
        {
            using var tx = session.BeginTransaction();
            List<UserCourses> dt = null;
            try
            {
                var cs = Query.Where(c => c.UserId == id).ToList();
                var ids = cs.Select(a => a.KhoaHocId);
                var listActiveById = _courseQr.Where(c => ids.Contains(c.Id) && c.Active).ToList();
                dt = cs.Where(c => listActiveById.Select(a => a.Id).Contains(c.KhoaHocId)).ToList();

                tx.Commit();
            }
            catch (Exception ex)
            {
                tx.Rollback();
            }
            return dt;
        }



    }
}
