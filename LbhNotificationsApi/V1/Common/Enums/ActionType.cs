using System.Text.Json.Serialization;

namespace LbhNotificationsApi.V1.Common.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ActionType
    {
        None, Initiated, Approved, Validate, Rejected,Removed
    }
}
