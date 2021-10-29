using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LbhNotificationsApi.V1.Common.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace LbhNotificationsApi.V1.Boundary.Requests
{
    public class NotificationSearchQuery
    {
        public Guid TargetId { get; set; }
        public NotificationType NotificationType { get; set; }
        public string User { get; set; }

    }
}
