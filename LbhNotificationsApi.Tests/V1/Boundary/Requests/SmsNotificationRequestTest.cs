using System.Collections.Generic;
using Bogus;
using FluentAssertions;
using LbhNotificationsApi.Tests.TestHelpers;
using LbhNotificationsApi.V1.Boundary.Requests;
using Xunit;


namespace LbhNotificationsApi.Tests.V1.Boundary.Requests
{

    public class SmsNotificationRequestTest
    {
        [Fact(DisplayName = "The notification request should have the correct properties")]
        public void NotificationRequestShouldHaveCorrectProperties()
        {
            var entityType = typeof(SmsNotificationRequest);
            entityType.GetProperties().Length.Should().Be(4);
            var entity = Fakr.Create<SmsNotificationRequest>();
            Assert.True(entity.HasProperty("MobileNumber"));
            Assert.True(entity.HasProperty("TemplateId"));
            Assert.True(entity.HasProperty("PersonalisationParams"));
            Assert.True(entity.HasProperty("ServiceKey"));
        }
    }
}
