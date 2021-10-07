using System;
using System.Threading.Tasks;
using LbhNotificationsApi.V1.Boundary.Response;
using LbhNotificationsApi.V1.Factories;
using LbhNotificationsApi.V1.Gateways.Interfaces;
using LbhNotificationsApi.V1.UseCase.Interfaces;

namespace NotificationsApi.V1.UseCase
{
    //TODO: Rename class name and interface name to reflect the entity they are representing eg. GetClaimantByIdUseCase
    public class GetByIdNotificationCase : IGetByIdNotificationCase
    {
        private INotificationGateway _gateway;
        public GetByIdNotificationCase(INotificationGateway gateway)
        {
            _gateway = gateway;
        }

        //TODO: rename id to the name of the identifier that will be used for this API, the type may also need to change
        public async Task<NotificationResponseObject> ExecuteAsync(Guid id)
        {
            var response = await _gateway.GetEntityByIdAsync(id).ConfigureAwait(false);
            return response?.ToResponse();
        }
    }
}
