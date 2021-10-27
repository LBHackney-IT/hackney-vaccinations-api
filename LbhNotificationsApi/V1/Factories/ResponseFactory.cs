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
                    //Id = domain.Id,
                    TargetId = domain.TargetId,
                    TargetType = domain.TargetType,
                    AuthorizedBy = domain.AuthorizedBy,
                    AuthorizedDate = domain.AuthorizedDate,
                    AuthorizerNote = domain.AuthorizerNote,
                    ApprovalStatus = domain.ApprovalStatus,
                    IsReadStatus = domain.IsReadStatus,
                    Message = domain.Message,
                    CreatedDate = domain.CreatedAt
                };
        }

        public static List<NotificationResponseObject> ToResponse(this IEnumerable<Notification> domainList)
        {
            return domainList.Select(domain => domain.ToResponse()).ToList();
        }
    }
}
