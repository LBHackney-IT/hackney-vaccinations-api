using System;
using Amazon.DynamoDBv2.DataModel;
using LbhNotificationsApi.V1.Common.Enums;
using LbhNotificationsApi.V1.Infrastructure.Conventers;

namespace LbhNotificationsApi.V1.Infrastructure
{

    [DynamoDBTable("notifications", LowerCamelCaseProperties = true)]
    public class NotificationEntity
    {
        [DynamoDBProperty(AttributeName = "id")]
        public Guid Id { get; set; }
        [DynamoDBHashKey]
        [DynamoDBProperty(AttributeName = "target_id")]
        public Guid TargetId { get; set; }

        [DynamoDBProperty(AttributeName = "target_type", Converter = typeof(DynamoDbEnumConverter<TargetType>))]
        public TargetType TargetType { get; set; }
        [DynamoDBProperty(AttributeName = "message")]
        public string Message { get; set; }
        [DynamoDBProperty(AttributeName = "is_read_status", Converter = typeof(DynamoDbBooleanConverter))]
        public bool IsReadStatus { get; set; }
        [DynamoDBProperty(AttributeName = "is_removed_status", Converter = typeof(DynamoDbBooleanConverter))]
        public bool IsRemovedStatus { get; set; }
        [DynamoDBProperty(AttributeName = "is_approval_required", Converter = typeof(DynamoDbBooleanConverter))]
        public bool IsApprovalRequired { get; set; }
        [DynamoDBProperty(AttributeName = "authorizer_note")]
        public string AuthorizerNote { get; set; }
        [DynamoDBProperty(AttributeName = "approval_status", Converter = typeof(DynamoDbEnumConverter<ApprovalStatus>))]
        public ApprovalStatus ApprovalStatus { get; set; }
        [DynamoDBProperty(AttributeName = "authorized_by")]
        public string AuthorizedBy { get; set; }
        [DynamoDBProperty(AttributeName = "authorized_date", Converter = typeof(DynamoDbDateTimeConverter))]
        public DateTime? AuthorizedDate { get; set; }
        [DynamoDBProperty(AttributeName = "created_at", Converter = typeof(DynamoDbDateTimeConverter))]
        public DateTime CreatedAt { get; set; }
    }
}
