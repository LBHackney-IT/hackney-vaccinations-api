using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using LbhNotificationsApi.V1.Boundary.Requests;
using LbhNotificationsApi.V1.Common.Enums;
using LbhNotificationsApi.V1.Domain;
using LbhNotificationsApi.V1.Factories;
using LbhNotificationsApi.V1.Gateways.Interfaces;
using LbhNotificationsApi.V1.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DocumentModel;

namespace LbhNotificationsApi.V1.Gateways
{
    public class DynamoDbGateway : INotificationGateway
    {
        private readonly IDynamoDBContext _dynamoDbContext;
        private readonly IAmazonDynamoDB _amazonDynamoDb;

        public DynamoDbGateway(IDynamoDBContext dynamoDbContext, IAmazonDynamoDB amazonDynamoDb)
        {
            _dynamoDbContext = dynamoDbContext;
            _amazonDynamoDb = amazonDynamoDb;
        }

        public async Task AddAsync(Notification notification)
        {
            var dbEntity = notification.ToDatabase();
            await _dynamoDbContext.SaveAsync(dbEntity).ConfigureAwait(false);

        }

        public async Task<List<Notification>> GetAllAsync()
        {
            var conditions = new List<ScanCondition>
            {
                new ScanCondition("Id", ScanOperator.NotEqual, Guid.Empty),
                new ScanCondition("IsRemovedStatus", ScanOperator.NotEqual, true),
                new ScanCondition("CreatedAt", ScanOperator.Between, DateTime.Today.Date, DateTime.Today.Date.AddMonths(3))
            };
            var data = await _dynamoDbContext.ScanAsync<NotificationEntity>(conditions).GetRemainingAsync().ConfigureAwait(false);
            return data.Select(x => x.ToDomain()).ToList();
        }


        public async Task<List<Notification>> GetAllAsync(NotificationSearchQuery query)
        {
            var conditions = new List<ScanCondition>
            {
                new ScanCondition("Id", ScanOperator.NotEqual, Guid.Empty),
                new ScanCondition("IsRemovedStatus", ScanOperator.NotEqual, true),
                new ScanCondition("CreatedAt", ScanOperator.Between, DateTime.Today.Date, DateTime.Today.Date.AddMonths(3))
            };
            if (query.NotificationType != NotificationType.All)
            {
                conditions.Add(new ScanCondition("NotificationType", ScanOperator.Equal, query.NotificationType));
            }
            if (!string.IsNullOrEmpty(query.User))
            {
                conditions.Add(new ScanCondition("User", ScanOperator.Equal, query.User));
            }

            if (query.TargetId != Guid.Empty)
            {
                conditions.Add(new ScanCondition("TargetId", ScanOperator.Equal, query.TargetId));
            }

            var data = await _dynamoDbContext.ScanAsync<NotificationEntity>(conditions).GetRemainingAsync().ConfigureAwait(false);
            return data.Select(x => x.ToDomain()).ToList();
        }


        public async Task<Notification> GetEntityByIdAsync(Guid id)
        {
            var result = await _dynamoDbContext.LoadAsync<NotificationEntity>(id).ConfigureAwait(false);
            //update data status as read
            result.IsReadStatus = true;
            await _dynamoDbContext.SaveAsync(result).ConfigureAwait(false);
            return result.ToDomain();
        }

        public async Task<Notification> UpdateAsync(Guid id, UpdateRequest notification)
        {
            var loadData = await _dynamoDbContext.LoadAsync<NotificationEntity>(id).ConfigureAwait(false);
            if (loadData == null) return null;

            if (!string.IsNullOrWhiteSpace(notification.ActionNote))
                loadData.ActionNote = notification.ActionNote;

            if (notification.ActionType == ActionType.Removed)
                loadData.IsRemovedStatus = true;

            loadData.PerformedActionType = notification.ActionType;
            loadData.PerformedActionDate = DateTime.UtcNow;
            await _dynamoDbContext.SaveAsync(loadData).ConfigureAwait(false);

            return loadData.ToDomain();
        }
    }
}
