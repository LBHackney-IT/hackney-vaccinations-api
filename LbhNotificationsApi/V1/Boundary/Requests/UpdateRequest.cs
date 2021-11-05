using LbhNotificationsApi.V1.Common.Enums;

namespace LbhNotificationsApi.V1.Boundary.Requests
{
    public class UpdateRequest
    {
        /// <summary>
        /// Type of ActionType [None, Initiated,Validate, Approved, Rejected]
        /// </summary>
        /// <example>
        /// None
        /// </example>
        public ActionType ActionType { get; set; }
        public string ActionNote { get; set; }
    }
}
