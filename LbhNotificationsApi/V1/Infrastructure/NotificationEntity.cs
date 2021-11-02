using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2.DataModel;
using LbhNotificationsApi.V1.Common.Enums;
using LbhNotificationsApi.V1.Infrastructure.Conventers;

namespace LbhNotificationsApi.V1.Infrastructure
{

    [DynamoDBTable("notifications", LowerCamelCaseProperties = true)]
    public class NotificationEntity
    {
        [DynamoDBProperty(AttributeName = "id")]
        [DynamoDBHashKey]
        public Guid Id { get; set; }
        
        [DynamoDBProperty(AttributeName = "target_id")]
        public Guid TargetId { get; set; }

        [DynamoDBProperty(AttributeName = "target_type", Converter = typeof(DynamoDbEnumConverter<TargetType>))]
        public TargetType TargetType { get; set; }
        [DynamoDBProperty(AttributeName = "notification_type", Converter = typeof(DynamoDbEnumConverter<TargetType>))]
        public NotificationType NotificationType { get; set; }
        [DynamoDBProperty(AttributeName = "user")]
        public string User { get; set; }
        [DynamoDBProperty(AttributeName = "message")]
        public string Message { get; set; }
        [DynamoDBProperty(AttributeName = "is_read_status", Converter = typeof(DynamoDbBooleanConverter))]
        public bool IsReadStatus { get; set; }
        [DynamoDBProperty(AttributeName = "is_removed_status", Converter = typeof(DynamoDbBooleanConverter))]
        public bool IsRemovedStatus { get; set; }
        [DynamoDBProperty(AttributeName = "is_approval_required", Converter = typeof(DynamoDbBooleanConverter))]
        public bool IsApprovalRequired { get; set; }
        [DynamoDBProperty(AttributeName = "action_note")]
        public string ActionNote { get; set; }
        //[DynamoDBProperty(AttributeName = "service_key")]
        //public string ServiceKey { get; set; }
        //[DynamoDBProperty(AttributeName = "template_Id")]
        //public string TemplateId { get; set; }
        [DynamoDBProperty(AttributeName = "mobile_number")]
        public string MobileNumber { get; set; }
        [DynamoDBProperty(AttributeName = "email")]
        public string Email { get; set; }
        [DynamoDBProperty(AttributeName = "require_action", Converter = typeof(DynamoDbBooleanConverter))]
        public bool RequireAction { get; set; }
        [DynamoDBProperty(AttributeName = "require_email_notification", Converter = typeof(DynamoDbBooleanConverter))]
        public bool RequireEmailNotification { get; set; }
        [DynamoDBProperty(AttributeName = "require_sms_notification", Converter = typeof(DynamoDbBooleanConverter))]
        public bool RequireSmsNotification { get; set; }
        [DynamoDBProperty(AttributeName = "require_letter", Converter = typeof(DynamoDbBooleanConverter))]
        public bool RequireLetter { get; set; }
        [DynamoDBProperty(AttributeName = "personalisation_params")]
        public Dictionary<string, string> PersonalisationParams { get; set; }
        [DynamoDBProperty(AttributeName = "action_performed", Converter = typeof(DynamoDbEnumConverter<ActionType>))]
        public ActionType ActionPerformed { get; set; }
        [DynamoDBProperty(AttributeName = "action_done_by")]
        public string ActionDoneBy { get; set; }
        [DynamoDBProperty(AttributeName = "action_date", Converter = typeof(DynamoDbDateTimeConverter))]
        public DateTime? ActionDate { get; set; }
        [DynamoDBProperty(AttributeName = "created_at", Converter = typeof(DynamoDbDateTimeConverter))]
        public DateTime CreatedAt { get; set; }
    }
}
