using System.Threading.Tasks;
using LbhNotificationsApi.V1.Boundary.Requests;
using LbhNotificationsApi.V1.Boundary.Response;

namespace LbhNotificationsApi.V1.UseCase.Interfaces
{
    public interface IGetAllNotificationCase
    {
        Task<NotificationResponseObjectList> ExecuteAsync(NotificationSearchQuery query);
    }
}
