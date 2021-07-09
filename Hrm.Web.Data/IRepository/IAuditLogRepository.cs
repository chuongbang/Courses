using Course.Web.Share.Domain;
using System.Linq;
using Course.Core.Patterns.Repository;

namespace Course.Data.IRepository
{
    public interface IAuditLogRepository: IRepository<AuditLog>
    {
        IQueryable<AuditLog> GetQueryable();
    }
}
