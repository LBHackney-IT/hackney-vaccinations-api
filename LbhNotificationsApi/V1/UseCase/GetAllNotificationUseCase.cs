using System.Threading.Tasks;
using LbhNotificationsApi.V1.Boundary.Requests;
using LbhNotificationsApi.V1.Boundary.Response;
using LbhNotificationsApi.V1.Factories;
using LbhNotificationsApi.V1.Gateways.Interfaces;
using LbhNotificationsApi.V1.UseCase.Interfaces;

namespace LbhNotificationsApi.V1.UseCase
{

    public class GetAllNotificationUseCase : IGetAllNotificationUseCase
    {
        private readonly INotificationGateway _gateway;
        public GetAllNotificationUseCase(INotificationGateway gateway)
        {
            _gateway = gateway;
        }

        public async Task<NotificationResponseObjectList> ExecuteAsync(NotificationSearchQuery query)
        {
            var response = await _gateway.GetAllAsync(query).ConfigureAwait(false);
            return new NotificationResponseObjectList { ResponseObjects = response.ToResponse() };
        }
    }
}
