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
    public class LessonsService : ILessonsService
    {
        private readonly ILessonsRepository _lessonsRepository;
        public LessonsService(ILessonsRepository LessonsRepository)
        {
            _lessonsRepository = LessonsRepository;
        }


        public async ValueTask<LessonsResult> GetByPageAsync(LessonsSearch cs, CallContext context = default)
        {
            List<LessonsData> dts = null;
            int total = 0;
            try
            {
               
                var data = await _lessonsRepository
                    .GetPageWithTransactionWithTotalAsync(c => c.TenBaiHoc.Contains(cs.Keyword ?? ""),
                    cs.Page.PageIndex -1, cs.Page.PageSize, c => c.TenBaiHoc, Core.Patterns.Repository.OrderType.Asc);

                dts = data.Item1?.Select(c => c.As<LessonsData>()).ToList();
                total = data.Item2;
            }
            catch (Exception ex)
            {
            }
            return new LessonsResult() { Dts = dts , Total = total};
        }        
        
        public async ValueTask<LessonsResult> GetAllActiveAsync(CallContext context = default)
        {
            List<LessonsData> dts = null;
            try
            {
                var data = await _lessonsRepository.GetAllActiveAsync();

                dts = data?.Select(c => c.As<LessonsData>()).ToList();
            }
            catch (Exception ex)
            {
            }
            return new LessonsResult() { Dts = dts};
        }


        public async ValueTask<ExcuteResponse> AddAsync(LessonsData hs, CallContext context = default)
        {
            try
            {
                var result = await _lessonsRepository.AddEntityAsync(hs.As<Lessons>());
                return new ExcuteResponse() { State = result };
            }
            catch (Exception)
            {
                return new ExcuteResponse() { State = false };
            }
        }

        public async ValueTask<ExcuteResponse> UpdateAsync(LessonsData hs, CallContext context = default)
        {
            try
            {
                var result = await _lessonsRepository.UpdateEntityAsync(hs.As<Lessons>());
                return new ExcuteResponse() { State = result };
            }
            catch (Exception)
            {
                return new ExcuteResponse() { State = false };
            }
        }

        public async ValueTask<ExcuteResponse> DeleteAsync(LessonsData hs, CallContext context = default)
        {
            try
            {
                var result = await _lessonsRepository.DeleteAsync(hs.Id);
                return new ExcuteResponse() { State = result };
            }
            catch (Exception)
            {
                return new ExcuteResponse() { State = false };
            }
        }

        public async ValueTask<ExcuteResponse> DeleteListAsync(List<LessonsData> ls, CallContext context = default)
        {
            try
            {
                var result = await _lessonsRepository.DeleteWithListKeyAsync(ls.Select(c => c.Id));
                return new ExcuteResponse() { State = result };
            }
            catch (Exception)
            {
                return new ExcuteResponse() { State = false };
            }
        }

    }
}
