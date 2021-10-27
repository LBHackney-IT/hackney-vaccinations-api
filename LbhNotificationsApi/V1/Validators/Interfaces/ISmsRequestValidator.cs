using LbhNotificationsApi.V1.Boundary.Requests;

namespace LbhNotificationsApi.V1.Validators.Interfaces
{
    public interface ISmsRequestValidator
    {
        bool ValidateSmsRequest(SmsNotificationRequest request);
    }
}
