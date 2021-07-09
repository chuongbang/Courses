using Course.Web.Share.Domain;
using Microsoft.AspNetCore.Http;
using NHibernate;
using NHibernate.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Course.Web.Share
{
    public class AuditEventListener : IPostUpdateEventListener, IPostDeleteEventListener, IPostInsertEventListener
    {
        private const string _noValueString = "*No Value*";
        // https://gnomealone.co.uk/2019/02/16/nhibernate-auditing/
        private readonly IHttpContextAccessor accessor;

        public AuditEventListener(IHttpContextAccessor accessor)
        {
            this.accessor = accessor;
        }

        public void OnPostDelete(PostDeleteEvent e)
        {
            var entity = e.Entity;
            if (!AuditEventListenerHelper.MapObjectName.ContainsKey(entity.GetType()) || e.DeletedState == null)
            {
                return;
            }
            e.UpdateAuditTrail(GetUsername(), e.DeletedState, new object[e.DeletedState.Length], "Xóa");
        }

        public void OnPostInsert(PostInsertEvent e)
        {
            var entity = e.Entity;
            if (!AuditEventListenerHelper.MapObjectName.ContainsKey(entity.GetType()) || e.State == null)
            {
                return;
            }
            e.UpdateAuditTrail(GetUsername(), new object[e.State.Length], e.State, "Thêm");
        }

        public void OnPostUpdate(PostUpdateEvent e)
        {
            var entity = e.Entity;
            if (!AuditEventListenerHelper.MapObjectName.ContainsKey(entity.GetType()) || e.OldState == null || e.State == null)
            {
                return;
            }
            e.UpdateAuditTrail(GetUsername(), e.OldState, e.State, "Cập nhật");
        }

        public Task OnPostUpdateAsync(PostUpdateEvent @event, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            OnPostUpdate(@event);
            return Task.CompletedTask;
        }

        public Task OnPostDeleteAsync(PostDeleteEvent @event, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            OnPostDelete(@event);
            return Task.CompletedTask;
        }

        public Task OnPostInsertAsync(PostInsertEvent @event, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            OnPostInsert(@event);
            return Task.CompletedTask;
        }

        string GetUsername() =>
            accessor?.HttpContext?.User?.Identity?.Name;
    }

    public static class AuditEventListenerHelper
    {
        public static readonly Dictionary<Type, string> MapObjectName = new Dictionary<Type, string>
        {
            //{typeof(Album),"Album" },
            //{typeof(AlbumImage),"Album ảnh" },
            //{typeof(Banners), "Banner" },
            //{typeof(Category), "Danh mục" },
            //{typeof(Contact), "Liên hệ" },
            //{typeof(GroupBanner), "Nhóm banner" },
            //{typeof(SiteConfig), "Cấu hình hệ thống" },
            //{typeof(Menu), "Menu" },
            {typeof(AppUser), "Tài khoản" },
            {typeof(AppRole), "Vai trò" },
            //{typeof(Link), "Liên kết website" },
            //{typeof(FileDocument), "Tệp tin hệ thống" },
            //{typeof(PinMap), "Hoạt động gìn giữ hòa bình" },
        };

        public static void UpdateAuditTrail(this AbstractPostDatabaseOperationEvent e, string username, object[] oldState, object[] newState, string action)
        {
            object entity = e.Entity;
            string id = e.Id.ToString();
            string[] propertyNames = e.Persister.PropertyNames;

            using NHibernate.ISession auditSession = e.Session.SessionWithOptions().AutoClose().FlushMode(FlushMode.Commit).OpenSession();
            using ITransaction auditTransaction = auditSession.BeginTransaction();
            var objTitle = GetRepresentativeName(entity);
            string title = objTitle != null ? objTitle.ToString() : entity.GetType().Name;
            if (title.Length > 255)
            {
                title = title.Remove(255);
            }
            
            var auditLog = new AuditLog
            {
                Id = Guid.NewGuid().ToString(),
                Username = username ?? "Unknown",
                Action = action,
                Timestamp = DateTime.Now,
                TableName = MapObjectName.ContainsKey(entity.GetType()) ? MapObjectName[entity.GetType()] : entity.GetType().Name,
                RecordId = id,
                Title = title
            };
            auditSession.Save(auditLog);
            for (var i = 0; i < newState.Length; i++)
            {
                string newValue = GetValueString(newState[i]);
                string oldValue = GetValueString(oldState[i]);
                if (newValue == oldValue)
                    continue;
                var auditLogDetail = new AuditLogDetail
                {
                    Id = Guid.NewGuid().ToString(),
                    AuditLog = auditLog,
                    FieldName = propertyNames[i],
                    OldValue = oldValue,
                    NewValue = newValue
                };
                auditSession.Save(auditLogDetail);
            }
            auditTransaction.Commit();
        }

        private static string GetValueString(object value)
        {
            if (value == null)
                return null;
            return value.ToString();

        }

        private static object GetRepresentativeName(object obj)
        {
            var props = obj.GetType().GetProperties().FirstOrDefault(prop => Attribute.IsDefined(prop, typeof(RepresentativeNameAttribute)));
            if (props != null)
            {
                return props.GetValue(obj);
            }
            return null;
        }

    }
}
