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
        public UserCoursesRepository(ISession session)
            : base(session)
        {
        }


        public async Task<List<UserCourses>> GetByIdAsync(string id)
        {
            using var tx = session.BeginTransaction();
            List<UserCourses> dt = null;
            try
            {
                dt = await Query.Where(c => c.UserId == id).ToListAsync();
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
