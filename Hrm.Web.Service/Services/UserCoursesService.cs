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
    public class UserCoursesService : IUserCoursesService
    {
        private readonly IUserCoursesRepository _UserCoursesRepository;
        IQueryable<UserCourses> _query;
        public UserCoursesService(IUserCoursesRepository UserCoursesRepository)
        {
            _UserCoursesRepository = UserCoursesRepository;
        }


        public async ValueTask<UserCoursesResult> GetByIdAsync(string id, CallContext context = default)
        {
            List<UserCoursesData> dts = new List<UserCoursesData>();
            int total = 0;
            try
            {
                var data = await _UserCoursesRepository.GetByIdAsync(id);
                dts = data?.Select(c => c.As<UserCoursesData>()).ToList();
            }
            catch (Exception ex)
            {
            }
            return new UserCoursesResult() { Dts = dts , Total = total};
        }        
        
        public async ValueTask<UserCoursesResult> GetAllActiveAsync(CallContext context = default)
        {
            List<UserCoursesData> dts = null;
            try
            {
               
                var data = await _UserCoursesRepository.GetAllAsync();

                dts = data?.Select(c => c.As<UserCoursesData>()).ToList();
            }
            catch (Exception ex)
            {
            }
            return new UserCoursesResult() { Dts = dts};
        }


        public async ValueTask<ExcuteResponse> AddAsync(UserCourseList hs, CallContext context = default)
        {
            try
            {
                var result = await _UserCoursesRepository.AddEntityAsync(hs.Dts.Select(c => c.As<UserCourses>()));
                return new ExcuteResponse() { State = result };
            }
            catch (Exception)
            {
                return new ExcuteResponse() { State = false };
            }
        }

        public async ValueTask<ExcuteResponse> UpdateAsync(UserCoursesData hs, CallContext context = default)
        {
            try
            {
                var result = await _UserCoursesRepository.UpdateEntityAsync(hs.As<UserCourses>());
                return new ExcuteResponse() { State = result };
            }
            catch (Exception)
            {
                return new ExcuteResponse() { State = false };
            }
        }

        public async ValueTask<ExcuteResponse> DeleteAsync(UserCoursesData hs, CallContext context = default)
        {
            try
            {
                var result = await _UserCoursesRepository.DeleteAsync(hs.Id);
                return new ExcuteResponse() { State = result };
            }
            catch (Exception)
            {
                return new ExcuteResponse() { State = false };
            }
        }

        //public async ValueTask<ExcuteResponse> DeleteListAsync(List<UserCoursesData> ls, CallContext context = default)
        //{
        //    try
        //    {
        //        var result = await _UserCoursesRepository.DeleteListAsync(ls.Select(c => c.As<UserCourses>()));
        //        return new ExcuteResponse() { State = result };
        //    }
        //    catch (Exception)
        //    {
        //        return new ExcuteResponse() { State = false };
        //    }
        //}

    }
}
