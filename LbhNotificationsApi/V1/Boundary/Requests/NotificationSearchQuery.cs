using LbhNotificationsApi.V1.Common.Enums;
using System;

namespace LbhNotificationsApi.V1.Boundary.Requests
{
    public class NotificationSearchQuery
    {
        public Guid TargetId { get; set; }
        public NotificationType NotificationType { get; set; }
        public string User { get; set; }

    }
}
