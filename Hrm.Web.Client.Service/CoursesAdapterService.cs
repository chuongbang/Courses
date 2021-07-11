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

        public async Task<bool> AddAsync(CoursesData hs)
        {
            try
            {
                _logger.LogInformation("{id}: Them ho so CoursesData", hs.Id);
                var response = await Service.AddAsync(hs);
                if (response != null)
                {
                    _logger.LogInformation("{id}: Them ho so CoursesData: {state}", hs.Id, response.State);
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

        public async Task<bool> UpdateAsync(CoursesData hs)
        {
            try
            {
                _logger.LogInformation("{id}: Cap nhat ho so CoursesData", hs.Id);
                var response = await Service.UpdateAsync(hs);
                if (response != null)
                {
                    _logger.LogInformation("{id}: Cap nhat ho so CoursesData: {state}", hs.Id, response.State);
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

        public async Task<bool> DeleteAsync(CoursesData hs)
        {
            try
            {
                _logger.LogInformation("{id}: Xoa ho so CoursesData [{key}] [{name}]", hs.Id);
                var response = await Service.DeleteAsync(hs);
                if (response != null)
                {
                    _logger.LogInformation("{id}: Xoa ho so CoursesData : {state}", hs.Id, response.State);
                    return response.State;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{id}: Xoa ho so CoursesData that bai [{key}] [{name}]",  hs.Id);
                throw;
            }
        }

        public async Task<CoursesResult> GetByIdsAsync(List<string> ids, Page page = null, string keyword = null)
        {
            try
            {
                page = page ?? new Page();
                return await Service.GetByIdsAsync(new CoursesSearch() { Ids = ids, Page = page , Keyword = keyword});
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Lay danh sach ho so CoursesModel that bai");
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
                _logger.LogInformation(ex, "Lay danh sach ho so CoursesModel that bai");
                throw;
            }
        }


        
        //public async Task<bool> DeleteListAsync(List<CoursesData> ls)
        //{
        //    try
        //    {
        //        _logger.LogInformation("{id}: Xoa list du lieu CoursesData", ThongTinToChucId);
        //        var response = await Service.DeleteListAsync(ls);
        //        if (response != null)
        //        {
        //            _logger.LogInformation("{id}:  Xoa list du lieu CoursesData: {state}", ThongTinToChucId, response.State);
        //            return response.State;
        //        }
        //        return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "{id}: Xoa list du lieu CoursesData that bai", ThongTinToChucId);
        //        throw;
        //    }
        //}



    }

}
