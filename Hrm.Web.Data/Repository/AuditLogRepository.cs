using Course.Data.IRepository;
using Course.Web.Share.Domain;
using NHibernate;
using System.Linq;
using Course.Core.Patterns.Repository;

namespace Course.Data.Repository
{
    public class AuditLogRepository : Repository<AuditLog>, IAuditLogRepository
    {
        public AuditLogRepository(ISession session) : base(session){ }
        public IQueryable<AuditLog> GetQueryable()
        {
            return session.Query<AuditLog>();
        }
    }
}
