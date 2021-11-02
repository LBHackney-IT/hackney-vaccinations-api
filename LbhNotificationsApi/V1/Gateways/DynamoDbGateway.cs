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
            var conditions = new List<ScanCondition>();
            var data = await _dynamoDbContext.ScanAsync<NotificationEntity>(conditions, null).GetRemainingAsync().ConfigureAwait(false);
            return data.Select(x => x.ToDomain()).ToList();
        }


        public async Task<List<Notification>> GetAllAsync(NotificationSearchQuery query)
        {
            var conditions = new List<ScanCondition>
            {
                new ScanCondition("Id", Amazon.DynamoDBv2.DocumentModel.ScanOperator.NotEqual, Guid.Empty)
            };
            if (query.NotificationType != NotificationType.All)
            {
                conditions.Add(new ScanCondition("NotificationType", Amazon.DynamoDBv2.DocumentModel.ScanOperator.Equal, query.NotificationType));
            }
            if (!string.IsNullOrEmpty(query.User))
            {
                conditions.Add(new ScanCondition("User", Amazon.DynamoDBv2.DocumentModel.ScanOperator.Equal, query.User));
            }

            if (query.TargetId != Guid.Empty)
            {
                conditions.Add(new ScanCondition("TargetId", Amazon.DynamoDBv2.DocumentModel.ScanOperator.Equal, query.TargetId));
            }

            var data = await _dynamoDbContext.ScanAsync<NotificationEntity>(conditions, null).GetRemainingAsync().ConfigureAwait(false);
            return data.Select(x => x.ToDomain()).ToList();
            //var request = new QueryRequest
            //{
            //    TableName = "notifications",
            //    KeyConditions = new Dictionary<string, Condition>
            //    {
            //        { "Id", new Condition()
            //            {
            //                ComparisonOperator = ComparisonOperator.EQ,
            //                AttributeValueList = new List<AttributeValue>
            //                {
            //                    new AttributeValue { N = "301" }
            //                }
            //            }
            //        }
            //    },
            //    ProjectionExpression = "Id, Title, #pr.ThreeStar",
            //    ExpressionAttributeNames = new Dictionary<string, string>
            //    {
            //        { "#pr", "ProductReviews" },
            //        { "#p", "Price" }
            //    },
            //    ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            //    {
            //        { ":val", new AttributeValue { N = "150" } }
            //    },
            //    FilterExpression = "#p > :val"
            //};
            //var response = client.Query(request);
        }
        

        public async Task<Notification> GetEntityByIdAsync(Guid id)
        {
            var result = await _dynamoDbContext.LoadAsync<NotificationEntity>(id).ConfigureAwait(false);
            return result?.ToDomain();
        }

        public async Task<Notification> UpdateAsync(Guid id, UpdateRequest notification)
        {
            var loadData = await _dynamoDbContext.LoadAsync<NotificationEntity>(id).ConfigureAwait(false);
            if (loadData == null) return null;

            if (!string.IsNullOrWhiteSpace(notification.ActionNote))
                loadData.ActionNote = notification.ActionNote;

            loadData.ActionPerformed = notification.ActionType;
            loadData.ActionDate = DateTime.UtcNow;
            await _dynamoDbContext.SaveAsync(loadData).ConfigureAwait(false);

            return loadData.ToDomain();
        }
    }
}
