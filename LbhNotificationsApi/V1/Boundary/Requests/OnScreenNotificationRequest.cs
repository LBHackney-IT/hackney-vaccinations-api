using System;
using LbhNotificationsApi.V1.Boundary.Response;
using LbhNotificationsApi.V1.Common.Enums;

namespace LbhNotificationsApi.V1.Boundary.Requests
{
    public class OnScreenNotificationRequest
    {
        public Guid TargetId { get; set; }
        public TargetType TargetType { get; set; }
        public string Message { get; set; }
        public string AdditionalMessage { get; set; }
        public NotificationDetailsObject NotificationObject { get; set; }
    }
}
