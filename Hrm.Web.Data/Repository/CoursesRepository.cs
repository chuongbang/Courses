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
using Course.Web.Share.IServices;

namespace Course.Web.Data.Repository
{
    public class CoursesRepository : Repository<Courses>, ICoursesRepository
    {
        IQueryable<Lessons> _lessonQuery;
        public CoursesRepository(ISession session)
            : base(session)
        {
            _lessonQuery = session.Query<Lessons>();
        }


        public async Task<(List<Courses>, int)> GetByIdsAsync(IEnumerable<string> ids, string keyword, int pageIndex, int pageSize)
        {
            using var tx = session.BeginTransaction();
            List<Courses> dt = null;
            int total = 0;
            try
            {
                var _query = Query;
                _query = _query.Where(c => ids.Contains(c.Id));
                if (keyword != null && keyword != string.Empty)
                {
                    _query = _query.Where(c => c.TenKhoaHoc.ToLower().Contains(keyword.ToLower()));
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

        public async Task<List<Courses>> GetAllActiveAsync()
        {
            using var tx = session.BeginTransaction();
            List<Courses> dt = null;
            try
            {
                dt = await Query.Where(c => c.IsActive).ToListAsync();
                tx.Commit();
            }
            catch (Exception ex)
            {
                tx.Rollback();
            }
            return dt;
        }

        public async Task<(List<Courses>, List<Lessons>, int)> GetCoursesActiveWithLessonsAsync(CoursesSearch cs)
        {
            using var tx = session.BeginTransaction();
            List<Courses> CDts = null;
            List<Lessons> LDts = null;
            int total = 0;
            try
            {
                var _query = Query;
                _query = _query.Where(c => c.IsActive);

                if (cs.Keyword != null && cs.Keyword != string.Empty)
                {
                    _query = _query.Where(c => c.TenKhoaHoc.ToLower().Contains(cs.Keyword.ToLower()));
                }
                total = _query.Count();

                CDts = await _query.Skip((cs.Page.PageIndex - 1) * cs.Page.PageSize).Take(cs.Page.PageSize).ToListAsync();

                var courseIds = CDts.Select(c => c.Id);
                LDts = await _lessonQuery.Where(c => courseIds.Contains(c.KhoaHocId)).ToListAsync();

                tx.Commit();

            }
            catch (Exception ex)
            {
                tx.Rollback();
            }

            return (CDts, LDts, total);
        }



    }
}
