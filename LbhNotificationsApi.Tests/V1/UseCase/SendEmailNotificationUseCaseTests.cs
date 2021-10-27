using LbhNotificationsApi.Tests.TestHelpers;
using LbhNotificationsApi.V1.Boundary.Requests;
using LbhNotificationsApi.V1.Gateways.Interfaces;
using LbhNotificationsApi.V1.UseCase;
using Moq;
using Xunit;

namespace LbhNotificationsApi.Tests.V1.UseCase
{
    public class SendEmailNotificationUseCaseTests
    {
        private readonly Mock<INotifyGateway> _mockGateway;
        private readonly SendEmailNotificationUseCase _classUnderTest;

        public SendEmailNotificationUseCaseTests()
        {
            _mockGateway = new Mock<INotifyGateway>();
            _classUnderTest = new SendEmailNotificationUseCase(_mockGateway.Object);
        }

        [Fact]
        public void UseCaseCallsGatewaySendEmail()
        {
            var request = Fakr.Create<EmailNotificationRequest>();
            _classUnderTest.Execute(request);
            _mockGateway.Verify(gw => gw.SendEmailNotification(It.IsAny<EmailNotificationRequest>()), Times.Once);
        }

    }
}
