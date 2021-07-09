using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AntDesign;

namespace Course.Web.Client.Ultils
{
    public static class AntNoticeExtentions
    {
        private static void NoticeWithIcon(this NotificationService notificationService, NotificationType type, string message)
        {
            _ = notificationService.Open(new NotificationConfig()
            {
                Message = "Thông báo",
                Description = message,
                NotificationType = type
            });
        }

        public static void NotiSuccess(this NotificationService notificationService, string message)
        {
            notificationService.NoticeWithIcon(NotificationType.Success, message);
        }

        public static void NotiWarning(this NotificationService notificationService, string message)
        {
            notificationService.NoticeWithIcon(NotificationType.Warning, message);
        }

        public static void NotiError(this NotificationService notificationService, string message)
        {
            notificationService.NoticeWithIcon(NotificationType.Error, message);
        }

        public static void NotiInfo(this NotificationService notificationService, string message)
        {
            notificationService.NoticeWithIcon(NotificationType.Info, message);
        }

        public static void NotiNone(this NotificationService notificationService, string message)
        {
            notificationService.NoticeWithIcon(NotificationType.None, message);
        }
    }
}
