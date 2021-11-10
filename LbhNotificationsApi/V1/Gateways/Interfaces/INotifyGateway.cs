using LbhNotificationsApi.V1.Boundary.Requests;
using Notify.Models.Responses;
using System.Threading.Tasks;

namespace LbhNotificationsApi.V1.Gateways.Interfaces
{
    public interface INotifyGateway
    {
        bool SendEmailNotification(EmailNotificationRequest request);
        bool SendTextMessageNotification(SmsNotificationRequest request);
        Task<TemplateList> GetTaskAllTemplateAsync();
    }
}
