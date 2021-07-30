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

        public bool CheckIsTrialAsync(string khoahocId)
        {
            bool isTrial = false;
            try
            {
                var uc = Query.FirstOrDefault(c => c.KhoaHocId == khoahocId);
                if (uc != null)
                {
                    isTrial = uc.IsTrial;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return isTrial;
        }

        public async Task<(List<UserCourses>, int)> GetByIdAsync(string userId, string keyword, int pageIndex, int pageSize)
        {
            using var tx = session.BeginTransaction();
            List<UserCourses> dt = null;
            int total = 0;
            try
            {
                var _query = Query;
                _query = _query.Where(c => c.UserId == userId);
                var khoaHocIds = _query.Select(a => a.KhoaHocId);
                var listActiveById = _courseQr.Where(c => khoaHocIds.Contains(c.Id) && c.IsActive);

                _query = _query.Where(c => listActiveById.Select(a => a.Id).Contains(c.KhoaHocId));
                total = _query.Count();
                dt = await _query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

                tx.Commit();
            }
            catch (Exception ex)
            {
                tx.Rollback();
            }
            return (dt, total);
        }

        public async Task<(List<Courses>, int)> GetPageByIdAsync(string userId, string keyword, int pageIndex, int pageSize)
        {
            using var tx = session.BeginTransaction();
            List<Courses> dt = null;
            int total = 0;
            try
            {
                var _query = Query;
                var listCourseByUserId = _query.Where(c => c.UserId == userId).Select(c => c.KhoaHocId).ToList();
                _courseQr = _courseQr.Where(c => listCourseByUserId.Contains(c.Id) && c.IsActive);
                if (keyword != null && keyword != string.Empty)
                {
                    _courseQr = _courseQr.Where(c => c.TenKhoaHoc.ToLower().Contains(keyword.ToLower()));
                }
                total = _courseQr.Count();

                dt = await _courseQr.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

                tx.Commit();
            }
            catch (Exception ex)
            {
                tx.Rollback();
            }
            return (dt, total);
        }


    }
}
