using Amazon.DynamoDBv2.Model;
using LbhNotificationsApi.V1.Domain;
using System;
using System.Collections.Generic;

namespace LbhNotificationsApi.V1.Factories
{
    public static class QueryResponseFactory
    {
        public static List<Notification> ToNotification(this QueryResponse response)
        {
            List<Notification> notifications = new List<Notification>();
            if (response is null)
            {
                return notifications;
            }



            foreach (Dictionary<string, AttributeValue> item in response.Items)
            {
                notifications.Add(new Notification
                {
                    Id = Guid.Parse(item["id"].S),
                    TargetId = item.ContainsKey("target_id") ? item["target_id"].S : null,
                    TargetType = item.ContainsKey("target_type") ? item["target_type"].S : null,
                    CreatedAt = DateTime.Parse(item["created_at"].S).ToUniversalTime(),
                    ActionNote = item.ContainsKey("action_note") ? item["action_note"].S : null,
                    PerformedActionDate = item.ContainsKey("performed_action_date") ? DateTime.Parse(item["performed_action_date"].S).ToUniversalTime() : (DateTime?) null,
                    PerformedActionDoneBy = item.ContainsKey("performed_action_done_by") ? item["performed_action_done_by"].S : null,
                    PerformedActionType = item.ContainsKey("performed_action_type") ? item["performed_action_type"].S : null,
                    RequireAction = bool.Parse(item["require_action"].S),
                    Email = item.ContainsKey("email") ? item["email"].S : null,
                    IsMessageSent = item.ContainsKey("is_message_sent") ? item["is_message_sent"].SS : null,
                    IsReadStatus = bool.Parse(item["is_read_status"].S),
                    RequireEmailNotification = bool.Parse(item["require_email_notification"].S),
                    RequireLetter = bool.Parse(item["require_letter"].S),
                    RequireSmsNotification = bool.Parse(item["require_sms_notification"].S),
                    PersonalisationParams = item.ContainsKey("personalisation_params") ? ToExpectedAttributeMap(item["personalisation_params"].M) : null,
                    Message = item.ContainsKey("message") ? item["message"].S : null,
                    MobileNumber = item.ContainsKey("mobile_number") ? item["mobile_number"].S : null,
                    NotificationType = item.ContainsKey("notification_type") ? item["notification_type"].S : null,
                    User = item.ContainsKey("user") ? item["user"].S : null
                });
            }

            return notifications;
        }


        public static Dictionary<string, string> ToExpectedAttributeMap(Dictionary<string, AttributeValue> resultItem)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            foreach (var kvp in resultItem)
            {
                ret.Add(kvp.Key, kvp.Value.S);
            }

            return ret;
        }
    }
}
