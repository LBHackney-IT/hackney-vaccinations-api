using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Util;
using LbhNotificationsApi.V1.Boundary.Requests;
using LbhNotificationsApi.V1.Common.Enums;
using LbhNotificationsApi.V1.Domain;
using LbhNotificationsApi.V1.Factories;
using LbhNotificationsApi.V1.Gateways.Interfaces;
using LbhNotificationsApi.V1.Infrastructure;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LbhNotificationsApi.V1.Gateways
{
    public class DynamoDbGateway : INotificationGateway
    {
        private readonly IDynamoDBContext _dynamoDbContext;
        private readonly IAmazonDynamoDB _amazonDynamoDb;
        private const string Pk = "lbhNoification";
        public DynamoDbGateway(IDynamoDBContext dynamoDbContext, IAmazonDynamoDB amazonDynamoDb)
        {
            _dynamoDbContext = dynamoDbContext;
            _amazonDynamoDb = amazonDynamoDb;
        }

        public async Task AddAsync(Notification notification)
        {
            var dbEntity = notification.ToDatabase();
            dbEntity.Pk = Pk;
            await _dynamoDbContext.SaveAsync(dbEntity).ConfigureAwait(false);

        }



        public async Task<List<Notification>> GetAllAsync(NotificationSearchQuery query)
        {

            DateTime startDate = DateTime.UtcNow;
            string start = startDate.ToString(AWSSDKUtils.ISO8601DateFormat);

            // You provide date value based on your test data.
            DateTime endDate = DateTime.UtcNow - TimeSpan.FromDays(90);
            string end = endDate.ToString(AWSSDKUtils.ISO8601DateFormat);
            QueryRequest queryRequest = new QueryRequest
            {
                TableName = "Notifications",
                KeyConditionExpression = "pk = :v1",
                FilterExpression = "created_at BETWEEN :v2a AND :v2b AND is_removed_status <> :v3",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                    {
                        {":v1", new AttributeValue {
                     S = Pk
                 }},
                {":v2a", new AttributeValue {
                     S = end
                 }},
                {":v2b", new AttributeValue {
                     S = start
                 }},
                 {":v3", new AttributeValue {
                     S = "true"
                 }}
                    }
            };

            if (query.NotificationType != NotificationType.All)
            {
                queryRequest.FilterExpression += " AND notification_type = :v4";
                queryRequest.ExpressionAttributeValues.Add(":v4", new AttributeValue { S = query.NotificationType.ToString() });
            }
            if (!string.IsNullOrEmpty(query.User))
            {
                queryRequest.FilterExpression += " AND #app_user = :v5";
                queryRequest.ExpressionAttributeValues.Add(":v5", new AttributeValue { S = query.User });
                queryRequest.ExpressionAttributeNames.Add("#app_user", "user");
            }

            if (query.TargetId != Guid.Empty)
            {
                queryRequest.FilterExpression += " AND target_id = :v6";
                queryRequest.ExpressionAttributeValues.Add(":v6", new AttributeValue { S = query.TargetId.ToString() });
            }
            var data = await _amazonDynamoDb.QueryAsync(queryRequest).ConfigureAwait(false);
            var results = data.ToNotification();
            return results;
        }


        public async Task<Notification> GetEntityByIdAsync(Guid id)
        {
            var result = await _dynamoDbContext.LoadAsync<NotificationEntity>(Pk, id).ConfigureAwait(false);
            if (result == null) return null;
            return result.ToDomain();
        }

        public async Task<Notification> UpdateAsync(Guid id, UpdateRequest notification)
        {
            var loadData = await _dynamoDbContext.LoadAsync<NotificationEntity>(Pk, id).ConfigureAwait(false);
            if (loadData == null) return null;

            if (!string.IsNullOrWhiteSpace(notification.ActionNote))
                loadData.ActionNote = notification.ActionNote;

            if (notification.ActionType == ActionType.IsRead)
                loadData.IsReadStatus = true;

            if (notification.ActionType == ActionType.Approved || notification.ActionType == ActionType.Rejected || notification.ActionType == ActionType.Validate)
            {

                loadData.PerformedActionType = notification.ActionType.ToString();
                loadData.PerformedActionDate = DateTime.UtcNow;
            }
            await _dynamoDbContext.SaveAsync(loadData).ConfigureAwait(false);

            return loadData.ToDomain();
        }


        public async Task<int> DeleteAsync(Guid id)
        {
            var loadData = await _dynamoDbContext.LoadAsync<NotificationEntity>(Pk, id).ConfigureAwait(false);
            if (loadData == null) return 0;
            if (loadData.RequireAction && string.IsNullOrEmpty(loadData.PerformedActionType))
                return -1;
            //throw new InvalidOperationException("You are not allow to remove/delete this record");

            loadData.IsRemovedStatus = true;
            await _dynamoDbContext.SaveAsync(loadData).ConfigureAwait(false);

            return 1;
        }
    }
}
