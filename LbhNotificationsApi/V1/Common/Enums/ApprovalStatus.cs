using System.Text.Json.Serialization;

namespace LbhNotificationsApi.V1.Common.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ApprovalStatus
    {
        Initiated, Approved, Rejected
    }
}
