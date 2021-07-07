using System.Linq;
using AutoFixture;
using LbhNotificationsApi.V1.Boundary.Response;
using LbhNotificationsApi.V1.Domain;
using LbhNotificationsApi.V1.Factories;
using LbhNotificationsApi.V1.Gateways;
using LbhNotificationsApi.V1.UseCase;
using FluentAssertions;
using LbhNotificationsApi.Tests.TestHelpers;
using LbhNotificationsApi.V1.Boundary.Requests;
using LbhNotificationsApi.V1.Gateways.Interfaces;
using Moq;
using NUnit.Framework;

namespace LbhNotificationsApi.Tests.V1.UseCase
{
    public class SendEmailNotificationUseCaseTests
    {
        private Mock<INotifyGateway> _mockGateway;
        private SendEmailNotificationUseCase _classUnderTest;

        [SetUp]
        public void SetUp()
        {
            _mockGateway = new Mock<INotifyGateway>();
            _classUnderTest = new SendEmailNotificationUseCase(_mockGateway.Object);
        }

        [Test]
        public void UseCaseCallsGatewaySendEmail()
        {
            var request = Fakr.Create<EmailNotificationRequest>();
            _classUnderTest.Execute(request);
            _mockGateway.Verify(gw => gw.SendEmailNotification(It.IsAny<EmailNotificationRequest>()), Times.Once);
        }

    }
}
