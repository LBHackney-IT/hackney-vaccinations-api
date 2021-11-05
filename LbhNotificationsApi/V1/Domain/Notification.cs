using System;
using System.Collections.Generic;
using LbhNotificationsApi.V1.Common.Enums;

namespace LbhNotificationsApi.V1.Domain
{

    public class Notification
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string TargetId { get; set; }
        public TargetType TargetType { get; set; }
        public NotificationType NotificationType { get; set; }
        public string User { get; set; }
        public string Message { get; set; }
        public string ServiceKey { get; set; }
        public string TemplateId { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public bool RequireAction { get; set; }
        public bool RequireEmailNotification { get; set; }
        public bool RequireSmsNotification { get; set; }
        public bool RequireLetter { get; set; }
        public bool IsReadStatus { get; set; }
        public List<string> IsMessageSent { get; set; }
        public string ActionNote { get; set; }
        public string PerformedActionDoneBy { get; set; }
        public ActionType PerformedActionType { get; set; }
        public Dictionary<string, string> PersonalisationParams { get; set; }
        public DateTime? PerformedActionDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
