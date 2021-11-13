using LbhNotificationsApi.V1.Boundary.Response;
using LbhNotificationsApi.V1.Gateways.Interfaces;
using LbhNotificationsApi.V1.UseCase.Interfaces;
using System.Threading.Tasks;

namespace LbhNotificationsApi.V1.UseCase
{
    public class GetTemplateByIdUseCase : IGetTemplateByIdUseCase
    {
        private readonly INotifyGateway _notifyGateway;
        public GetTemplateByIdUseCase(INotifyGateway notifyGateway)
        {
            _notifyGateway = notifyGateway;
        }

        public async Task<NotifyTemplate> ExecuteAsync(string id, string service)
        {
            var response = await _notifyGateway.GetTemplateByAsync(id, service).ConfigureAwait(false);
            return response;
        }
    }
}
