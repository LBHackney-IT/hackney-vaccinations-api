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
            ActionResponse result = response switch
            {
                -1 => new ActionResponse { Status = false, Message = "You are not allow to remove/delete this record" },
                0 => new ActionResponse { Status = false, Message = $"Record with Id: {id} not found" },
                1 => new ActionResponse { Status = true, Message = "successfully removed" },

                _ => new ActionResponse { Status = false, Message = "Unknow error, please try again later" },
            };

            return result;
        }
    }
}
