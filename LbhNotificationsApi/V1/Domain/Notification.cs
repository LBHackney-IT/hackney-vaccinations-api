using System;
using LbhNotificationsApi.V1.Common.Enums;

namespace LbhNotificationsApi.V1.Domain
{

    public class Notification
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid TargetId { get; set; }
        public TargetType TargetType { get; set; }
        public string Message { get; set; }
        public bool IsReadStatus { get; set; }
        public string AuthorizerNote { get; set; }
        public string AuthorizedBy { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        public DateTime? AuthorizedDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
