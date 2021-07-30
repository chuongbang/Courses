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
    public class UserCoursesAdapterService : BaseAdapterService<IUserCoursesService>
    {
        public UserCoursesAdapterService(HostService host, Course.Core.AuditLog.FileLoggerProvider fileLoggerProvider, TokenProvider tokenProvider)
            : base(host, fileLoggerProvider, tokenProvider)
        {
        }

        public async Task<bool> AddAsync(List<UserCoursesData> hs)
        {
            try
            {
                _logger.LogInformation("Them danh sach UserCoursesData");
                var response = await Service.AddAsync(new UserCourseList() { Dts = hs});
                if (response != null)
                {
                    _logger.LogInformation(" danh sach UserCoursesData: {state}", response.State);
                    return response.State;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Them ho so that bai");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(UserCoursesData hs)
        {
            try
            {
                _logger.LogInformation("{id}: Cap nhat ho so UserCoursesData", hs.Id);
                var response = await Service.UpdateAsync(hs);
                if (response != null)
                {
                    _logger.LogInformation("{id}: Cap nhat ho so UserCoursesData: {state}", hs.Id, response.State);
                    return response.State;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{id}: Them ho so that bai", hs.Id);
                throw;
            }

        }

        public async Task<bool> DeleteAsync(UserCoursesData hs)
        {
            try
            {
                _logger.LogInformation("{id}: Xoa ho so UserCoursesData", hs.Id);
                var response = await Service.DeleteAsync(hs);
                if (response != null)
                {
                    _logger.LogInformation("{id}: Xoa ho so UserCoursesData : {state}", hs.Id, response.State);
                    return response.State;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{id}: Xoa ho so UserCoursesData that bai",  hs.Id);
                throw;
            }
        }

        public async Task<UserCoursesResult> GetByIdAsync(string id, Page page = null, string keyword = null)
        {
            try
            {
                return await Service.GetByIdAsync(new CoursesSearch() { Page = page, Keyword = keyword, Id = id });
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Lay danh sach ho so UserCoursesModel that bai");
                throw;
            }
        }
        public async Task<CoursesResult> GetPageByIdAsync(string id, Page page = null, string keyword = null)
        {
            try
            {
                return await Service.GetByPageWithUserIdAsync(new CoursesSearch() { Page = page, Keyword = keyword, Id = id });
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Lay danh sach ho so UserCoursesModel that bai");
                throw;
            }
        }            
        
        public bool CheckIsTrialAsync(string khaohocId)
        {
            try
            {
                return Service.CheckIsTrialAsync(khaohocId).State;
            }
            catch (Exception ex)
            {
                throw;
            }
        }        



    }

}
