using System;
using System.Threading.Tasks;
using LbhNotificationsApi.V1.Boundary.Response;

namespace LbhNotificationsApi.V1.UseCase.Interfaces
{
    public interface IGetTargetDetailsCase
    {
        Task<NotificationDetailsObject> ExecuteAsync(Guid targetId);
    }
}
