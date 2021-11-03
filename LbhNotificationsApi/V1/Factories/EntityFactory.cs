using LbhNotificationsApi.V1.Domain;
using LbhNotificationsApi.V1.Infrastructure;

namespace LbhNotificationsApi.V1.Factories
{
    public static class EntityFactory
    {
        public static Notification ToDomain(this NotificationEntity databaseEntity)
        {

            return new Notification
            {
                Id = databaseEntity.Id,
                TargetId = databaseEntity.TargetId,
                TargetType = databaseEntity.TargetType,
                Message = databaseEntity.Message,
                PerformedActionType = databaseEntity.PerformedActionType,
                PerformedActionDoneBy = databaseEntity.PerformedActionDoneBy,
                PerformedActionDate = databaseEntity.PerformedActionDate,
                ActionNote = databaseEntity.ActionNote,
                IsReadStatus = databaseEntity.IsReadStatus,
                CreatedAt = databaseEntity.CreatedAt,
                RequireAction = databaseEntity.RequireAction,
                Email = databaseEntity.Email,
                RequireEmailNotification = databaseEntity.RequireEmailNotification,
                PersonalisationParams = databaseEntity.PersonalisationParams,
                //IsRemovedStatus = false,
                //TemplateId = "",
                //ServiceKey = "",
                RequireLetter = databaseEntity.RequireLetter,
                RequireSmsNotification = databaseEntity.RequireSmsNotification,
                NotificationType = databaseEntity.NotificationType,
                MobileNumber = databaseEntity.MobileNumber,
                User = databaseEntity.User
            };
        }

        public static NotificationEntity ToDatabase(this Notification entity)
        {

            return new NotificationEntity
            {
                Id = entity.Id,
                TargetId = entity.TargetId,
                TargetType = entity.TargetType,
                Message = entity.Message,
                PerformedActionType = entity.PerformedActionType,
                PerformedActionDoneBy = entity.PerformedActionDoneBy,
                PerformedActionDate = entity.PerformedActionDate,
                ActionNote = entity.ActionNote,
                PersonalisationParams = entity.PersonalisationParams,
                IsReadStatus = entity.IsReadStatus,
                CreatedAt = entity.CreatedAt,
                RequireAction = entity.RequireAction,
                Email = entity.Email,
                RequireEmailNotification = entity.RequireEmailNotification,
                IsRemovedStatus = false,
                //TemplateId = "",
                //ServiceKey = "",
                RequireLetter = entity.RequireLetter,
                RequireSmsNotification = entity.RequireSmsNotification,
                NotificationType = entity.NotificationType,
                MobileNumber = entity.MobileNumber,
                User = entity.User
            };
        }
    }
}
