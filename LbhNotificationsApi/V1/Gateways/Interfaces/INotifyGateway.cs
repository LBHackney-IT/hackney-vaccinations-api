using LbhNotificationsApi.V1.Boundary.Requests;

namespace LbhNotificationsApi.V1.Gateways.Interfaces
{
    public interface INotifyGateway
    {
        bool SendEmailNotification(EmailNotificationRequest request);
        bool SendTextMessageNotification(SmsNotificationRequest request);
    }
}
