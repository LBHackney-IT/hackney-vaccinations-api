using System.Text.Json.Serialization;

namespace LbhNotificationsApi.V1.Common.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ActionType
    {
        Initiated, Approved, Validate, Rejected, IsRead
    }
}
