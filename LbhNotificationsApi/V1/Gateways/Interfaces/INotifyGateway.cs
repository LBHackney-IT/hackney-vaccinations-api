using LbhNotificationsApi.V1.Boundary.Requests;
using LbhNotificationsApi.V1.Boundary.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LbhNotificationsApi.V1.Gateways.Interfaces
{
    public interface INotifyGateway
    {
        bool SendEmailNotification(EmailNotificationRequest request);
        bool SendTextMessageNotification(SmsNotificationRequest request);
        Task<IEnumerable<NotifyTemplate>> GetTaskAllTemplateAsync(string serviceKey);
    }
}
