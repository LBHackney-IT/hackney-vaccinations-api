using System.Text.Json.Serialization;

namespace LbhNotificationsApi.V1.Common.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TargetType
    {
        None,
        FailedDirectDebits,
        MissedServiceCharge,
        EstimatesAndActuals,
        ViewNewProperty,
        ViewNewTenancy,
        SuspenseAccount,
        GlobalCharges,
        Adjustments
    }
}
