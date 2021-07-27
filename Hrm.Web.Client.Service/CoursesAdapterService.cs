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
    public class CoursesAdapterService : BaseAdapterService<ICoursesService>
    {
        public CoursesAdapterService(HostService host, Course.Core.AuditLog.FileLoggerProvider fileLoggerProvider, TokenProvider tokenProvider)
            : base(host, fileLoggerProvider, tokenProvider)
        {
        }

        public async Task<ExcuteResponse> AddAsync(CoursesData hs)
        {
            return await Service.AddAsync(hs);
        }

        public async Task<ExcuteResponse> UpdateAsync(CoursesData hs)
        {
            return await Service.UpdateAsync(hs);
        }

        public async Task<ExcuteResponse> DeleteAsync(CoursesData hs)
        {
            return await Service.DeleteAsync(hs);
        }

        public async Task<ExcuteResponse> DeleteListAsync(List<CoursesData> hs)
        {
            return await Service.DeleteListAsync(hs);
        }

        public async Task<CoursesResult> GetByPageAsync(Page page = null, string keyword = null, string userId = null)
        {
            try
            {
                page = page ?? new Page();
                if (userId != null)
                {
                    return await Service.GetByPageWithUserIdAsync(new CoursesSearch() { Page = page, Keyword = keyword, Id = userId });
                }
                return await Service.GetByPageAsync(new CoursesSearch() { Page = page, Keyword = keyword });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<CoursesResult> GetAllActiveAsync()
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

        public async Task<CourseLessons> GetCoursesActiveWithLessonsAsync(Page page = null, string keyword = null)
        {
            try
            {
                page = page ?? new Page();
                return await Service.GetCoursesActiveWithLessonsAsync(new CoursesSearch() { Page = page, Keyword = keyword });
            }
            catch (Exception ex)
            {
                throw;
            }
        }



    }

}
