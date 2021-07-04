using System.Threading.Tasks;
using LbhNotificationsApi.V1.Boundary.Requests;

namespace LbhNotificationsApi.V1.Gateways.Interfaces
{
    public interface INotifyGateway
    {
        bool SendEmailConfirmation(EmailNotificationRequest request);
        bool SendTextMessageConfirmation(SmsNotificationRequest request);
    }
}
