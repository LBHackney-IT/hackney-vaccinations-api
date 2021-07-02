using System.Collections.Generic;
using System.Threading.Tasks;
using HackneyVaccinationsApi.V1.Controllers;
using HackneyVaccinationsApi.V1.UseCase;
using FluentAssertions;
using HackneyVaccinationsApi.Tests.TestHelpers;
using HackneyVaccinationsApi.V1.Boundary.Requests;
using HackneyVaccinationsApi.V1.UseCase.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace HackneyVaccinationsApi.Tests.V1.Controllers
{

    [TestFixture]
    public class ConfirmationsControllerTests
    {
        private ConfirmationsController _classUnderTest;
        private Mock<ISendConfirmationUseCase> _mockUseCase;
        [SetUp]
        public void SetUp()
        {
            _mockUseCase = new Mock<ISendConfirmationUseCase>();
            _classUnderTest = new ConfirmationsController(_mockUseCase.Object);
        }

        [TestCase(TestName = "Confirmations controller post request returns response with status")]
        public async Task ReturnsResponseWithStatus()
        {
            var confirmationRequest = Fakr.Create<ConfirmationRequest>();
            var response = await _classUnderTest.CreateConfirmations(confirmationRequest).ConfigureAwait(true) as CreatedResult;
            response.StatusCode.Should().Be(201);
        }

        [TestCase(TestName = "Confirmations controller post request calls the send confirmation use case")]
        public async Task CallsSendConfirmationUseCase()
        {
            var confirmationRequest = Fakr.Create<ConfirmationRequest>();
            await _classUnderTest.CreateConfirmations(confirmationRequest).ConfigureAwait(true);
            _mockUseCase.Verify(u => u.Execute(It.IsAny<ConfirmationRequest>()), Times.Once);
        }
    }
}
