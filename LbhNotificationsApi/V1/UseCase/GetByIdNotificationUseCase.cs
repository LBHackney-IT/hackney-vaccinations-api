using System;
using System.Threading.Tasks;
using LbhNotificationsApi.V1.Boundary.Response;
using LbhNotificationsApi.V1.Factories;
using LbhNotificationsApi.V1.Gateways.Interfaces;
using LbhNotificationsApi.V1.UseCase.Interfaces;

namespace LbhNotificationsApi.V1.UseCase
{
    public class GetByIdNotificationUseCase : IGetByIdNotificationUseCase
    {
        private INotificationGateway _gateway;
        public GetByIdNotificationUseCase(INotificationGateway gateway)
        {
            _gateway = gateway;
        }

        public async Task<NotificationResponseObject> ExecuteAsync(Guid id)
        {
            var response = await _gateway.GetEntityByIdAsync(id).ConfigureAwait(false);
            return response?.ToResponse();
        }
    }
}
