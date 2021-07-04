using System.Threading.Tasks;
using LbhNotificationsApi.V1.Boundary.Requests;
using LbhNotificationsApi.V1.Gateways.Interfaces;
using LbhNotificationsApi.V1.UseCase.Interfaces;

namespace LbhNotificationsApi.V1.UseCase
{
    public class SendConfirmationUseCase : ISendConfirmationUseCase
    {
        private readonly INotifyGateway _notifyGateway;

        public SendConfirmationUseCase(INotifyGateway gateway)
        {
            _notifyGateway = gateway;
        }
        public async Task Execute(ConfirmationRequest request)
        {
            if (!string.IsNullOrWhiteSpace(request.Email))
            {
                await _notifyGateway.SendEmailConfirmation(request.Email, request.BookingSlot).ConfigureAwait(true);
            }

            if (!string.IsNullOrWhiteSpace(request.MobileNumber))
            {
                await _notifyGateway.SendTextMessageConfirmation(request.MobileNumber, request.BookingSlot).ConfigureAwait(true);
            }
        }
    }
}
