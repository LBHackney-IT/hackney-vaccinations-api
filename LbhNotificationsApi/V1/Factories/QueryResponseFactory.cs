using Amazon.DynamoDBv2.Model;
using LbhNotificationsApi.V1.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LbhNotificationsApi.V1.Factories
{
    public class QueryResponseFactory
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
                    TargetId = item["target_id"].S,
                    TargetType = item["target_type"].S,
                    CreatedAt = DateTime.Parse(item["statement_period_end_date"].S).ToUniversalTime(),
                    ActionNote = item["rent_account_number"].S,
                    PerformedActionDate = DateTime.Parse(item["statement_period_end_date"].S).ToUniversalTime(),
                    PerformedActionDoneBy = item["statement_type"].S,
                    PerformedActionType = item["rent_account_number"].S,
                    RequireAction = item["rent_account_number"].S,
                    Email = item["rent_account_number"].S
                    IsMessageSent = item["rent_account_number"].S,
                    IsReadStatus = item["rent_account_number"].S,
                    Message= item["rent_account_number"].S,
                    MobileNumber= item["rent_account_number"].S,
                    NotificationType= item["rent_account_number"].S,
                    User= 
                });
            }

            return statements;
        }
    }
}
