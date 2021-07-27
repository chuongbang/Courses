using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Course.Web.Share.Domain;
using Course.Web.Share.IServices;
using Course.Web.Client.Service;
using Course.Web.Share;
using Course.Web.Client.Models;

namespace Course.Web.Client.Data
{
    public class LessonsAdapterService : BaseAdapterService<ILessonsService>
    {
        public LessonsAdapterService(HostService host, Course.Core.AuditLog.FileLoggerProvider fileLoggerProvider, TokenProvider tokenProvider)
            : base(host, fileLoggerProvider, tokenProvider)
        {
        }

        public async Task<ExcuteResponse> AddAsync(LessonsData hs)
        {
            return await Service.AddAsync(hs);
        }

        public async Task<ExcuteResponse> UpdateAsync(LessonsData hs)
        {
            try
            {
                return await Service.UpdateAsync(hs);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<ExcuteResponse> DeleteAsync(LessonsData hs)
        {
            return await Service.DeleteAsync(hs);
        }

        public async Task<ExcuteResponse> DeleteListAsync(List<LessonsData> hs)
        {
            return await Service.DeleteListAsync(hs);
        }

        public async Task<LessonsResult> GetByPageAsync(Page page = null, string keyword = null)
        {
            try
            {
                page = page ?? new Page();
                return await Service.GetByPageAsync(new LessonsSearch() { Page = page, Keyword = keyword });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<LessonsResult> GetAllActiveAsync()
        {
            try
            {
                return await Service.GetAllActiveAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<LessonsResult> GetLessonsByCourseId(string courseId)
        {
            try
            {
                return await Service.GetLessonsByCourseId(courseId);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


    }

}
