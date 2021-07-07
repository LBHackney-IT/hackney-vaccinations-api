using System.Threading.Tasks;
using LbhNotificationsApi.V1.Boundary.Requests;
using LbhNotificationsApi.V1.Gateways.Interfaces;
using LbhNotificationsApi.V1.UseCase.Interfaces;

namespace LbhNotificationsApi.V1.UseCase
{
    public class SendEmailNotificationUseCase : ISendEmailNotificationUseCase
    {
        private readonly INotifyGateway _notifyGateway;

        public SendEmailNotificationUseCase(INotifyGateway gateway)
        {
            _notifyGateway = gateway;
        }
        public void Execute(EmailNotificationRequest request)
        {
            if (!string.IsNullOrWhiteSpace(request.Email))
            {
                _notifyGateway.SendEmailNotification(request);
            }
        }
    }
}
