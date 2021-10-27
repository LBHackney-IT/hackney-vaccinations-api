using LbhNotificationsApi.Tests.TestHelpers;
using LbhNotificationsApi.V1.Boundary.Requests;
using LbhNotificationsApi.V1.Gateways.Interfaces;
using LbhNotificationsApi.V1.UseCase;
using Moq;
using Xunit;


namespace LbhNotificationsApi.Tests.V1.UseCase
{
    public class SendSmsNotificationUseCaseTests
    {
        private readonly Mock<INotifyGateway> _mockGateway;
        private readonly SendSmsNotificationUseCase _classUnderTest;

        public SendSmsNotificationUseCaseTests()
        {
            _mockGateway = new Mock<INotifyGateway>();
            _classUnderTest = new SendSmsNotificationUseCase(_mockGateway.Object);
        }

        [Fact]
        public void UseCaseCallsGatewaySendEmail()
        {
            var request = Fakr.Create<SmsNotificationRequest>();
            _classUnderTest.Execute(request);
            _mockGateway.Verify(gw => gw.SendTextMessageNotification(It.IsAny<SmsNotificationRequest>()), Times.Once);
        }
    }
}
