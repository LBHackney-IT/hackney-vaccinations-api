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
    public class CreateConfirmationUseCaseTests
    {
        private Mock<INotifyGateway> _mockGateway;
        private SendConfirmationUseCase _classUnderTest;

        [SetUp]
        public void SetUp()
        {
            _mockGateway = new Mock<INotifyGateway>();
            _classUnderTest = new SendConfirmationUseCase(_mockGateway.Object);
        }

        [Test]
        public void UseCaseCallsGatewaySendEmail()
        {
            var request = Fakr.Create<ConfirmationRequest>();
            _classUnderTest.Execute(request);
            _mockGateway.Verify(gw => gw.SendEmailConfirmation(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void UseCaseCallsGatewaySendText()
        {
            var request = Fakr.Create<ConfirmationRequest>();
            _classUnderTest.Execute(request);
            _mockGateway.Verify(gw => gw.SendTextMessageConfirmation(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

    }
}
