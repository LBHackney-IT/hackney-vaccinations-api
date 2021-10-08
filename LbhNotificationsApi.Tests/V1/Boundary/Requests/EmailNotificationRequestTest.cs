using FluentAssertions;
using LbhNotificationsApi.Tests.TestHelpers;
using LbhNotificationsApi.V1.Boundary.Requests;
using Xunit;

namespace LbhNotificationsApi.Tests.V1.Boundary.Requests
{

    public class EmailNotificationRequestTest
    {
        [Fact(DisplayName = "The notification request should have the correct properties")]
        public void NotificationRequestShouldHaveCorrectProperties()
        {
            var entityType = typeof(EmailNotificationRequest);
            entityType.GetProperties().Length.Should().Be(4);
            var entity = Fakr.Create<EmailNotificationRequest>();
            Assert.True(entity.HasProperty("ServiceKey"));
            Assert.True(entity.HasProperty("TemplateId"));
            Assert.True(entity.HasProperty("Email"));
            Assert.True(entity.HasProperty("PersonalisationParams"));
            Assert.True(entity.HasProperty("ServiceKey"));
        }



    }
}
