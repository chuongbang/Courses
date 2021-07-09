﻿using NHibernate;
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
    public class CoursesRepository : Repository<Courses>, ICoursesRepository
    {
        public CoursesRepository(ISession session)
            : base(session)
        {
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
                dt = await _query.Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();

                tx.Commit();
            }
            catch (Exception ex)
            {
                tx.Rollback();
            }
            return (dt, total);
        }


        //public async Task<bool> DeleteListAsync(IEnumerable<Courses> dts)
        //{
        //    BeginTransaction();
        //    try
        //    {
        //        await DeleteWithListKeyAsync(dts?.Select(c => c.Id), false);
        //        return await CommitTransactionAsync();
        //    }
        //    catch (Exception)
        //    {
        //        await RolbackTransactionAsync();
        //        return false;
        //    }
        //}
    }
}