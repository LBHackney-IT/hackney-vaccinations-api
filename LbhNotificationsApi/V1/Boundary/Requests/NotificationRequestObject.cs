using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using LbhNotificationsApi.V1.Boundary.Response;
using LbhNotificationsApi.V1.Common.Enums;

namespace LbhNotificationsApi.V1.Boundary.Requests
{
    public class NotificationRequestObject
    {
        public Guid TargetId { get; set; }
        public TargetType TargetType { get; set; }
        public NotificationType NotificationType { get; set; }
        public string User { get; set; }
        public string Message { get; set; }
        public string ServiceKey { get; set; }
        public string TemplateId { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public bool RequireAction { get; set; }
        public bool RequireEmailNotification { get; set; } = false;
        public bool RequireSmsNotification { get; set; } = false;
        public bool RequireLetter { get; set; } = false;
        [JsonPropertyName("personalisationParams")]
        public Dictionary<string, string> PersonalisationParams { get; set; }
    }
}
