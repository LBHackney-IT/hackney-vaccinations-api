using System;
using System.Threading.Tasks;
using LbhNotificationsApi.V1.Boundary.Requests;
using LbhNotificationsApi.V1.Boundary.Response;
using LbhNotificationsApi.V1.Gateways.Interfaces;
using LbhNotificationsApi.V1.UseCase.Interfaces;

namespace LbhNotificationsApi.V1.UseCase
{
    public class UpdateNotificationUseCase : IUpdateNotificationUseCase
    {
        private readonly INotificationGateway _gateway;

        public UpdateNotificationUseCase(INotificationGateway gateway)
        {
            _gateway = gateway;
        }

        public async Task<ActionResponse> ExecuteAsync(Guid id, UpdateRequest request)
        {
            var notification = await _gateway.UpdateAsync(id, request).ConfigureAwait(false);

            var status = notification != null && notification.PerformedActionDate.HasValue && (notification.PerformedActionDate.Value.Date == DateTime.Today.Date);

            return new ActionResponse { Status = status, Message = status ? "action was successfully" : "action failed" };
        }
    }
}
