using System.Collections.Generic;
using Bogus;
using FluentAssertions;
using LbhNotificationsApi.Tests.TestHelpers;
using LbhNotificationsApi.V1.Boundary.Requests;
using NUnit.Framework;

namespace LbhNotificationsApi.Tests.V1.Boundary.Requests
{
    [TestFixture]
    public class SmsNotificationRequestTest
    {
        [TestCase(TestName = "The notification request should have the correct properties")]
        public void NotificationRequestShouldHaveCorrectProperties()
        {
            var entityType = typeof(SmsNotificationRequest);
            entityType.GetProperties().Length.Should().Be(4);
            var entity = Fakr.Create<SmsNotificationRequest>();
            Assert.That(entity, Has.Property("MobileNumber").InstanceOf(typeof(string)));
            Assert.That(entity, Has.Property("ServiceKey").InstanceOf(typeof(string)));
            Assert.That(entity, Has.Property("TemplateId").InstanceOf(typeof(string)));
            Assert.That(entity, Has.Property("PersonalisationParams").InstanceOf(typeof(Dictionary<string, string>)));
        }
    }
}
