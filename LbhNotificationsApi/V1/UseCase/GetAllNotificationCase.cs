using System.Threading.Tasks;
using LbhNotificationsApi.V1.Boundary.Response;
using LbhNotificationsApi.V1.Factories;
using LbhNotificationsApi.V1.Gateways.Interfaces;
using LbhNotificationsApi.V1.UseCase.Interfaces;

namespace NotificationsApi.V1.UseCase
{

    public class GetAllNotificationCase : IGetAllNotificationCase
    {
        private readonly INotificationGateway _gateway;
        public GetAllNotificationCase(INotificationGateway gateway)
        {
            _gateway = gateway;
        }

        public async Task<NotificationResponseObjectList> ExecuteAsync()
        {
            var response = await _gateway.GetAllAsync().ConfigureAwait(false);
            return new NotificationResponseObjectList { ResponseObjects = response.ToResponse() };
        }
    }
}
