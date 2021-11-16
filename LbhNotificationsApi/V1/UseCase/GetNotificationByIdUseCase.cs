using LbhNotificationsApi.V1.Boundary.Response;
using LbhNotificationsApi.V1.Gateways.Interfaces;
using System.Threading.Tasks;

namespace LbhNotificationsApi.V1.UseCase
{
    public class GetNotificationByIdUseCase : IGetNotificationByIdUseCase
    {
        private readonly INotifyGateway _notifyGateway;
        public GetNotificationByIdUseCase(INotifyGateway notifyGateway)
        {
            _notifyGateway = notifyGateway;
        }

        public async Task<GovNotificationResponse> ExecuteAsync(string id, string service)
        {
            var response = await _notifyGateway.GetNotificationByIdAsync(id, service).ConfigureAwait(false);
            return response;
        }
    }
}
