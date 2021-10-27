using System;
using System.Threading.Tasks;
using LbhNotificationsApi.V1.Boundary.Response;
using LbhNotificationsApi.V1.UseCase.Interfaces;

namespace NotificationsApi.V1.UseCase
{
    public class GetTargetDetailsCase : IGetTargetDetailsCase
    {
        public Task<NotificationDetailsObject> ExecuteAsync(Guid targetId)
        {
            throw new NotImplementedException();
        }
    }
}
