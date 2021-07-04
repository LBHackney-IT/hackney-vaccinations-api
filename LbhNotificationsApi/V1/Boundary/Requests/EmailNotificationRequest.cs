using System.Text.Json.Serialization;

namespace LbhNotificationsApi.V1.Boundary.Requests
{
    public class EmailNotificationRequest : NotificationRequest
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }
    }
}
