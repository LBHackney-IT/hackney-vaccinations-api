using System.Collections.Generic;
using System.Linq;
using LbhNotificationsApi.V1.Boundary.Response;
using LbhNotificationsApi.V1.Domain;

namespace LbhNotificationsApi.V1.Factories
{
    public static class ResponseFactory
    {

        public static NotificationResponseObject ToResponse(this Notification domain)
        {
            return domain == null
                ? null
                : new NotificationResponseObject
                {
                    Id = domain.Id,
                    TargetId = domain.TargetId,
                    TargetType = domain.TargetType,
                    ActionDoneBy = domain.ActionDoneBy,
                    ActionDate = domain.ActionDate,
                    ActionNote = domain.ActionNote,
                    ActionPerform = domain.ActionPerformed,
                    IsReadStatus = domain.IsReadStatus,
                    Message = domain.Message,
                    CreatedDate = domain.CreatedAt,
                    RequireAction = domain.RequireAction,
                    Email = domain.Email,
                    RequireEmailNotification = domain.RequireEmailNotification,
                    SentMessageDetails = domain.IsMessageSent,
                    //TemplateId = domain.TemplateId,
                    //ServiceKey = domain.ServiceKey,
                    RequireLetter = domain.RequireLetter,
                    MobileNumber = domain.MobileNumber,
                    NotificationType = domain.NotificationType,
                    RequireSmsNotification = domain.RequireSmsNotification,
                    PersonalisationParams = domain.PersonalisationParams
                };
        }

        public static List<NotificationResponseObject> ToResponse(this IEnumerable<Notification> domainList)
        {
            return domainList.Select(domain => domain.ToResponse()).ToList();
        }
    }
}
