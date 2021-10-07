using LbhNotificationsApi.V1.Common.Enums;

namespace LbhNotificationsApi.V1.Boundary.Requests
{
    public abstract class ApprovalRequest
    {
        public ApprovalStatus ApprovalStatus { get; set; }
        public string ApprovalNote { get; set; }
    }
}
