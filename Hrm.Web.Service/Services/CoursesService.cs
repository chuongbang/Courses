using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using Course.Web.Data.IRepository;
using Course.Web.Share.Domain;
using Course.Core.Extentions;
using Course.Web.Share.IServices;
using Course.Web.Share;

namespace Course.Web.Service.Services
{
    public class CoursesService : ICoursesService
    {
        private readonly ICoursesRepository _coursesRepository;
        public CoursesService(ICoursesRepository coursesRepository)
        {
            _coursesRepository = coursesRepository;
        }


        public async ValueTask<CoursesResult> GetByIdsAsync(CoursesSearch cs, CallContext context = default)
        {
            List<CoursesData> dts = null;
            int total = 0;
            try
            {
               
                var data = await _coursesRepository.GetByIdsAsync(cs.Ids, cs.Keyword, cs.Page.PageIndex, cs.Page.PageSize);

                dts = data.Item1?.Select(c => c.As<CoursesData>()).ToList();
                total = data.Item2;
            }
            catch (Exception ex)
            {
            }
            return new CoursesResult() { Dts = dts , Total = total};
        }        
        
        public async ValueTask<CoursesResult> GetAllActiveAsync(CallContext context = default)
        {
            List<CoursesData> dts = null;
            try
            {
                var data = await _coursesRepository.GetAllActiveAsync();

                dts = data?.Select(c => c.As<CoursesData>()).ToList();
            }
            catch (Exception ex)
            {
            }
            return new CoursesResult() { Dts = dts};
        }


        public async ValueTask<ExcuteResponse> AddAsync(CoursesData hs, CallContext context = default)
        {
            try
            {
                var result = await _coursesRepository.AddEntityAsync(hs.As<Courses>());
                return new ExcuteResponse() { State = result };
            }
            catch (Exception)
            {
                return new ExcuteResponse() { State = false };
            }
        }

        public async ValueTask<ExcuteResponse> UpdateAsync(CoursesData hs, CallContext context = default)
        {
            try
            {
                var result = await _coursesRepository.UpdateEntityAsync(hs.As<Courses>());
                return new ExcuteResponse() { State = result };
            }
            catch (Exception)
            {
                return new ExcuteResponse() { State = false };
            }
        }

        public async ValueTask<ExcuteResponse> DeleteAsync(CoursesData hs, CallContext context = default)
        {
            try
            {
                var result = await _coursesRepository.DeleteAsync(hs.Id);
                return new ExcuteResponse() { State = result };
            }
            catch (Exception)
            {
                return new ExcuteResponse() { State = false };
            }
        }

        //public async ValueTask<ExcuteResponse> DeleteListAsync(List<CoursesData> ls, CallContext context = default)
        //{
        //    try
        //    {
        //        var result = await _coursesRepository.DeleteListAsync(ls.Select(c => c.As<Courses>()));
        //        return new ExcuteResponse() { State = result };
        //    }
        //    catch (Exception)
        //    {
        //        return new ExcuteResponse() { State = false };
        //    }
        //}

    }
}
