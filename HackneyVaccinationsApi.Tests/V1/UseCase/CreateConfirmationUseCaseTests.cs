using System.Linq;
using AutoFixture;
using HackneyVaccinationsApi.V1.Boundary.Response;
using HackneyVaccinationsApi.V1.Domain;
using HackneyVaccinationsApi.V1.Factories;
using HackneyVaccinationsApi.V1.Gateways;
using HackneyVaccinationsApi.V1.UseCase;
using FluentAssertions;
using HackneyVaccinationsApi.Tests.TestHelpers;
using HackneyVaccinationsApi.V1.Boundary.Requests;
using HackneyVaccinationsApi.V1.Gateways.Interfaces;
using Moq;
using NUnit.Framework;

namespace HackneyVaccinationsApi.Tests.V1.UseCase
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
