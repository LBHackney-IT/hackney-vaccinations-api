using HackneyVaccinationsApi.V1.Boundary.Requests;
using HackneyVaccinationsApi.V1.Gateways.Interfaces;
using HackneyVaccinationsApi.V1.UseCase.Interfaces;

namespace HackneyVaccinationsApi.V1.UseCase
{
    public class SendConfirmationUseCase : ISendConfirmationUseCase
    {
        private readonly INotifyGateway _notifyGateway;

        public SendConfirmationUseCase(INotifyGateway gateway)
        {
            _notifyGateway = gateway;
        }
        public void Execute(ConfirmationRequest request)
        {
            if (!string.IsNullOrWhiteSpace(request.Email))
            {
                _notifyGateway.SendEmailConfirmation(request.Email, request.BookingSlot);
            }

            if (!string.IsNullOrWhiteSpace(request.MobileNumber))
            {
                _notifyGateway.SendTextMessageConfirmation(request.MobileNumber, request.BookingSlot);
            }
        }
    }
}
