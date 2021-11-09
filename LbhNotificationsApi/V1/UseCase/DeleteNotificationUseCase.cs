using LbhNotificationsApi.V1.Boundary.Response;
using LbhNotificationsApi.V1.Gateways.Interfaces;
using System;
using System.Threading.Tasks;

namespace LbhNotificationsApi.V1.UseCase
{
    public class DeleteNotificationUseCase : IDeleteNotificationUseCase
    {
        private readonly INotificationGateway _gateway;

        public DeleteNotificationUseCase(INotificationGateway gateway)
        {
            _gateway = gateway;
        }

        public async Task<ActionResponse> ExecuteAsync(Guid id)
        {
            var response = await _gateway.DeleteAsync(id).ConfigureAwait(false);

            var status = response != null;

            return new ActionResponse { Status = status, Message = status ? "successfully removed" : "remove action failed" };
        }
    }
}
