using LbhNotificationsApi.V1.Boundary.Response;
using LbhNotificationsApi.V1.Gateways.Interfaces;
using LbhNotificationsApi.V1.UseCase.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LbhNotificationsApi.V1.UseCase
{

    public class GetAllTemplateUseCase : IGetAllTemplateUseCase
    {
        private readonly INotifyGateway _notifyGateway;
        public GetAllTemplateUseCase(INotifyGateway notifyGateway)
        {
            _notifyGateway = notifyGateway;
        }

        public async Task<IEnumerable<NotifyTemplate>> ExecuteAsync(string query)
        {
            var response = await _notifyGateway.GetAllTemplateAsync(query).ConfigureAwait(false);
            return response;
        }
    }
}
