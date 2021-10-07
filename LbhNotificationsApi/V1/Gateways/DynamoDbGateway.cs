using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using LbhNotificationsApi.V1.Boundary.Requests;
using LbhNotificationsApi.V1.Domain;
using LbhNotificationsApi.V1.Factories;
using LbhNotificationsApi.V1.Gateways.Interfaces;
using LbhNotificationsApi.V1.Infrastructure;

namespace LbhNotificationsApi.V1.Gateways
{
    public class DynamoDbGateway : INotificationGateway
    {
        private readonly IDynamoDBContext _dynamoDbContext;

        public DynamoDbGateway(IDynamoDBContext dynamoDbContext)
        {
            _dynamoDbContext = dynamoDbContext;
        }

        public async Task AddAsync(Notification notification)
        {
            var dbEntity = notification.ToDatabase();
            await _dynamoDbContext.SaveAsync(dbEntity).ConfigureAwait(false);

        }

        public async Task<List<Notification>> GetAllAsync()
        {
            var conditions = new List<ScanCondition>();
            var data = await _dynamoDbContext.ScanAsync<NotificationEntity>(conditions, null).GetRemainingAsync().ConfigureAwait(false);
            return data.Select(x => x.ToDomain()).ToList();
        }

        public Notification GetEntityById(Guid id)
        {
            var result = _dynamoDbContext.LoadAsync<NotificationEntity>(id).GetAwaiter().GetResult();
            return result?.ToDomain();
        }

        public async Task<Notification> GetEntityByIdAsync(Guid id)
        {
            var result = await _dynamoDbContext.LoadAsync<NotificationEntity>(id).ConfigureAwait(false);
            return result?.ToDomain();
        }

        public async Task<Notification> UpdateAsync(Guid id, ApprovalRequest notification)
        {
            var loadData = await _dynamoDbContext.LoadAsync<NotificationEntity>(id).ConfigureAwait(false);
            if (loadData == null) return null;

            if (!string.IsNullOrWhiteSpace(notification.ApprovalNote))
                loadData.AuthorizerNote = notification.ApprovalNote;

            loadData.ApprovalStatus = notification.ApprovalStatus;
            loadData.AuthorizedDate = DateTime.UtcNow;
            await _dynamoDbContext.SaveAsync(loadData).ConfigureAwait(false);

            return loadData.ToDomain();
        }
    }
}
