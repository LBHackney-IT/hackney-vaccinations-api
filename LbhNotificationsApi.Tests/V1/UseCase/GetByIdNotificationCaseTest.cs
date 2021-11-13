using System;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using LbhNotificationsApi.V1.Boundary.Response;
using LbhNotificationsApi.V1.Domain;
using LbhNotificationsApi.V1.Factories;
using LbhNotificationsApi.V1.Gateways.Interfaces;
using LbhNotificationsApi.V1.UseCase;
using Moq;
using Xunit;

namespace LbhNotificationsApi.Tests.V1.UseCase
{
    public class GetByIdNotificationCaseTest
    {
        private readonly Mock<INotificationGateway> _mockGateway;
        private readonly GetByIdNotificationUseCase _classUnderTest;
        private readonly Fixture _fixture;


        public GetByIdNotificationCaseTest()
        {
            _mockGateway = new Mock<INotificationGateway>();
            _classUnderTest = new GetByIdNotificationUseCase(_mockGateway.Object);
            _fixture = new Fixture();
        }

        [Fact]
        public async Task GetByIdUseCaseShouldReturnOkResponse()
        {
            var stubbedEntity = _fixture.Create<Notification>();
            _mockGateway.Setup(x => x.GetEntityByIdAsync(stubbedEntity.Id)).ReturnsAsync(stubbedEntity);
            var expectedResponse = stubbedEntity.ToResponse();

            var response = await _classUnderTest.ExecuteAsync(stubbedEntity.Id).ConfigureAwait(false);
            response.Should().NotBeNull();
            response.Should().BeEquivalentTo(expectedResponse);

        }

        [Fact]
        public async Task GetByIdUseCaseShouldBeNull()
        {

            var id = Guid.NewGuid();
            _mockGateway.Setup(x => x.GetEntityByIdAsync(id)).ReturnsAsync((Notification) null);


            var response = await _classUnderTest.ExecuteAsync(id).ConfigureAwait(false);
            response.Should().BeNull();

        }

        [Fact]
        public void GetByIdThrowsException()
        {
            var request = Guid.NewGuid();
            var exception = new ApplicationException("Test Exception");
            _mockGateway.Setup(x => x.GetEntityByIdAsync(request)).ThrowsAsync(exception);
            Func<Task<NotificationResponseObject>> throwException = async () => await _classUnderTest.ExecuteAsync(request).ConfigureAwait(false);
            throwException.Should().Throw<ApplicationException>().WithMessage("Test Exception");
        }
    }
}
