using System.Text.Json.Serialization;

namespace LbhNotificationsApi.V1.Boundary.Requests
{
    public class SmsNotificationRequest : NotificationRequest
    {
        [JsonPropertyName("mobileNumber")]
        public string MobileNumber { get; set; }
    }
}
