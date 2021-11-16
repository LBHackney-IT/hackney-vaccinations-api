using LbhNotificationsApi.V1.Boundary.Response;
using System.Threading.Tasks;

namespace LbhNotificationsApi.V1.UseCase
{
    public interface IGetNotificationByIdUseCase
    {
        Task<GovNotificationResponse> ExecuteAsync(string id, string service);
    }
}
