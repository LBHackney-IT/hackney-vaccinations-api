using System;

namespace LbhNotificationsApi.V1.Boundary.Response
{
    public class GovNotificationResponse
    {
        public string Id { get; set; }
        public string CompletedAt { get; set; }
        public string EmailAddress { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Line3 { get; set; }
        public string Line4 { get; set; }
        public string Line5 { get; set; }
        public string Line6 { get; set; }
        public string PhoneNumber { get; set; }
        public string Postage { get; set; }
        public string Reference { get; set; }
        public string SentAt { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public string CreatedByName { get; set; }
        public Template Template { get; set; }
    }

    public class Template
    {
        public string Id { get; set; }
        public string Path { get; set; }
        public int Version { get; set; }
    }
}
