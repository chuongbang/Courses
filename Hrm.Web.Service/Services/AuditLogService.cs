using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using NHibernate;
using NHibernate.AspNetCore.Identity;
using NHibernate.Linq;
using Course.Data.IRepository;
using Course.Data.Repository;
using Course.Web.Share.Domain;
using Course.Web.Share.IServices;
using Course.Web.Share.Ultils;

namespace Course.Web.Service.Services
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AuditLogService : IAuditLogService
    {
        readonly ILogger<AuditLog> _logger;
        readonly IMapper _mapper;
        readonly IAuditLogRepository _auditLogRepository;

        public AuditLogService(
            ILogger<AuditLog> logger,
            IMapper mapper,
            IAuditLogRepository auditLogRepository
            )
        {
            _logger = logger;
            _mapper = mapper;
            _auditLogRepository = auditLogRepository;
        }

        public async Task<ListAuditLogResult> GetPageWithFilterAsync(AuditLogSearch model)
        {
            try
            {
                _auditLogRepository.BeginTransaction();
                var filter = model.CreateFilter(_auditLogRepository.GetQueryable());
                var pageData = await _auditLogRepository.GetPageWithTotalAsync(filter, model.Page.PageIndex - 1, model.Page.PageSize, c => c.Timestamp, Core.Patterns.Repository.OrderType.Desc);
                var itemIds = pageData.Item1.Select(e => e.Id).ToList();
                var data = await _auditLogRepository.GetQueryable().Fetch(c => c.AuditLogDetails).Where(c => itemIds.Contains(c.Id)).OrderByDescending(c => c.Timestamp).ToListAsync();
                var result = new ListAuditLogResult
                {
                    Data = _mapper.Map<List<AuditLogData>>(data),
                    Total = pageData.Item2
                };
                await _auditLogRepository.CommitTransactionAsync();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ListAuditLogResult();
            }
        }
    }
}
