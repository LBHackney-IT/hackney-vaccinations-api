using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LbhNotificationsApi.V1.Boundary.Requests
{
    public abstract class NotificationRequest
    {
        [JsonPropertyName("serviceKey")]
        public string ServiceKey { get; set; }
        [JsonPropertyName("templateId")]
        public string TemplateId { get; set; }
        [JsonPropertyName("personalisationParams")]
        public Dictionary<string, string> PersonalisationParams { get; set; }
    }
}
