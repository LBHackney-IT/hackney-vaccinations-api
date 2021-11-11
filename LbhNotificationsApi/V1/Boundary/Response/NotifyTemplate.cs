using System;

namespace LbhNotificationsApi.V1.Boundary.Response
{
    public class NotifyTemplate
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
        public string Type { get; set; }
        public string Body { get; set; }
        public int Version { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string LetterContactBlock { get; set; }
        public string CreatedBy { get; set; }
    }
}
