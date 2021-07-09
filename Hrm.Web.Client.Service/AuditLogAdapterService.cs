using Course.Data.IRepository;
using Course.Web.Share.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using NHibernate.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Course.Web.Share;
using Course.Core.AuditLog;
using Course.Web.Share.IServices;
using System.Security.Claims;

namespace Course.Web.Client.Service
{
    public class AuditLogAdapterService : BaseAdapterService<IAuditLogService>
    {
        public AuditLogAdapterService(HostService host, FileLoggerProvider fileLoggerProvider, TokenProvider tokenProvider)
            : base(host, fileLoggerProvider, tokenProvider)
        {
        }

        public async Task<ListAuditLogResult> GetPageAsync(AuditLogSearch auditLogSearch)
        {
            return await Service.GetPageWithFilterAsync(auditLogSearch);
        }

    }

}
