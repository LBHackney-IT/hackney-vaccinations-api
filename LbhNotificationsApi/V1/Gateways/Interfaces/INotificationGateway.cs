using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LbhNotificationsApi.V1.Boundary.Requests;
using LbhNotificationsApi.V1.Domain;

namespace LbhNotificationsApi.V1.Gateways.Interfaces
{
    public interface INotificationGateway
    {
        Task<Notification> GetEntityByIdAsync(Guid id);

        Task<List<Notification>> GetAllAsync();
        Task<List<Notification>> GetAllAsync(NotificationSearchQuery query);
        Task AddAsync(Notification notification);

        Task<Notification> UpdateAsync(Guid id, ApprovalRequest notification);
    }
}
