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
    public class LessonsRepository : Repository<Lessons>, ILessonsRepository
    {
        IQueryable<Courses> _queryCourse;
        public LessonsRepository(ISession session)
            : base(session)
        {
            _queryCourse = session.Query<Courses>();
        }


        public async Task<(List<Lessons>, int)> GetByIdsAsync(IEnumerable<string> ids, string keyword, int pageIndex, int pageSize)
        {
            using var tx = session.BeginTransaction();
            List<Lessons> dt = null;
            int total = 0;
            try
            {
                var _query = Query;
                _query = _query.Where(c => ids.Contains(c.Id));
                if (keyword != null && keyword != string.Empty)
                {
                    _query = _query.Where(c => c.TenBaiHoc.ToLower().Contains(keyword.ToLower()));
                }
                total = _query.Count();
                if (pageIndex != 0)
                {
                    dt = await _query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
                }
                else
                {
                    dt = await _query.ToListAsync();
                }
                tx.Commit();
            }
            catch (Exception ex)
            {
                tx.Rollback();
            }
            return (dt, total);
        }

        public async Task<List<Lessons>> GetAllActiveAsync()
        {
            using var tx = session.BeginTransaction();
            List<Lessons> dt = null;
            try
            {
                dt = await Query.ToListAsync();
                tx.Commit();
            }
            catch (Exception ex)
            {
                tx.Rollback();
            }
            return dt;
        }      
        public async Task<(List<Lessons>, Courses)> GetLessonsByCourseId(string courseId)
        {
            using var tx = session.BeginTransaction();
            List<Lessons> dts = null;
            Courses course = null;
            try
            {
                course = await _queryCourse.FirstOrDefaultAsync(c => c.Id == courseId);
                dts = await Query.Where(c => c.KhoaHocId == courseId).ToListAsync();
                tx.Commit();
            }
            catch (Exception ex)
            {
                tx.Rollback();
            }
            return (dts,course);
        }



    }
}
