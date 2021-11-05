using AutoFixture;
using LbhNotificationsApi.V1.Domain;
using LbhNotificationsApi.V1.Infrastructure;

namespace LbhNotificationsApi.Tests.V1.Helper
{
    public static class DatabaseEntityHelper
    {
        public static NotificationEntity CreateDatabaseEntity()
        {
            var entity = new Fixture().Create<Notification>();

            return CreateDatabaseEntityFrom(entity);
        }

        public static NotificationEntity CreateDatabaseEntityFrom(Notification entity)
        {
            return new NotificationEntity
            {
                Id = entity.Id,
                TargetId = entity.TargetId,
                TargetType = entity.TargetType,
                RequireAction = entity.RequireAction,
                PerformedActionDoneBy = entity.PerformedActionDoneBy,
                PerformedActionDate = entity.PerformedActionDate,
                ActionNote = entity.ActionNote,
                PerformedActionType = entity.PerformedActionType,
                IsReadStatus = entity.IsReadStatus,
                Message = entity.Message,
                CreatedAt = entity.CreatedAt,
            };
        }
    }
}
