using System.Threading.Tasks;
using LbhNotificationsApi.V1.Boundary.Requests;
using LbhNotificationsApi.V1.Gateways.Interfaces;
using LbhNotificationsApi.V1.UseCase.Interfaces;

namespace LbhNotificationsApi.V1.UseCase
{
    public class SendSmsNotificationUseCase : ISendSmsNotificationUseCase
    {
        private readonly INotifyGateway _notifyGateway;

        public SendSmsNotificationUseCase(INotifyGateway gateway)
        {
            _notifyGateway = gateway;
        }
        public void Execute(SmsNotificationRequest request)
        {

            if (!string.IsNullOrWhiteSpace(request.MobileNumber))
            {
                _notifyGateway.SendTextMessageNotification(request);
            }
        }
    }
}
