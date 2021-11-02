using System.Text.Json.Serialization;

namespace LbhNotificationsApi.V1.Common.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TargetType
    {
        FailedDirectDebits,
        MissedServiceCharge,
        Estimates,
        Actuals,
        ViewNewProperty,
        ValuateNewProperty,
        ViewNewTenancy,
        SuspenseAccount
    }
}
