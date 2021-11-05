using LbhNotificationsApi.V1.Common.Enums;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LbhNotificationsApi.V1.Boundary.Requests
{
    public class NotificationRequestObject
    {
        /// <example>
        /// 3fa85f64-5717-4562-b3fc-2c963f66afa6
        /// </example>
        public string TargetId { get; set; }
        /// <summary>
        /// Type of TargetType [None,FailedDirectDebits,MissedServiceCharge,EstimatesAndActuals,ViewNewProperty,ViewNewTenancy,SuspenseAccount,GlobalCharges,Adjustments]
        /// </summary>
        /// <example>
        /// None
        /// </example>
        public TargetType TargetType { get; set; }
        /// <summary>
        /// Type of NotificationType [All, Screen, Text, Email,Letter]
        /// </summary>
        /// <example>
        /// All
        /// </example>
        public NotificationType NotificationType { get; set; }
        public string User { get; set; }
        public string Message { get; set; }
        public string ServiceKey { get; set; }
        public string TemplateId { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// /// <example>
        /// false
        /// </example>
        public bool RequireAction { get; set; }
        /// <example>
        /// false
        /// </example>
        public bool RequireEmailNotification { get; set; }
        /// <example>
        /// false
        /// </example>
        public bool RequireSmsNotification { get; set; }
        /// <example>
        /// false
        /// </example>
        public bool RequireLetter { get; set; } = false;
        [JsonPropertyName("personalisationParams")]
        public Dictionary<string, string> PersonalisationParams { get; set; }
    }
}
