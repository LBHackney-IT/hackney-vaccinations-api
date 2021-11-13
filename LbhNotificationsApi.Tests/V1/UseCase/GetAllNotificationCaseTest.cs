using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using LbhNotificationsApi.V1.Boundary.Requests;
using LbhNotificationsApi.V1.Boundary.Response;
using LbhNotificationsApi.V1.Domain;
using LbhNotificationsApi.V1.Factories;
using LbhNotificationsApi.V1.Gateways.Interfaces;
using LbhNotificationsApi.V1.UseCase;
using Moq;
using Xunit;

namespace LbhNotificationsApi.Tests.V1.UseCase
{
    public class GetAllNotificationCaseTest
    {
        private readonly Mock<INotificationGateway> _mockGateway;
        private readonly GetAllNotificationUseCase _classUnderTest;
        private readonly Fixture _fixture;

        public GetAllNotificationCaseTest()
        {
            _mockGateway = new Mock<INotificationGateway>();
            _classUnderTest = new GetAllNotificationUseCase(_mockGateway.Object);
            _fixture = new Fixture();
        }

        [Fact]
        public async Task GetsAllFromTheGateway()
        {
            var stubbedEntities = _fixture.CreateMany<Notification>().ToList();
            var query = new NotificationSearchQuery();
            _mockGateway.Setup(x => x.GetAllAsync(query)).ReturnsAsync(stubbedEntities);

            var responseObjects = stubbedEntities.ToResponse();
            var expectedResponse = new NotificationResponseObjectList { ResponseObjects = responseObjects };

            var response = await _classUnderTest.ExecuteAsync(query).ConfigureAwait(false);
            response.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task GetSingleOneFromTheGateway()
        {
            var stubbedEntities = _fixture.CreateMany<Notification>(1).ToList();
            var query = new NotificationSearchQuery();
            _mockGateway.Setup(x => x.GetAllAsync(query)).ReturnsAsync(stubbedEntities);
            var expectedResponse = new NotificationResponseObjectList { ResponseObjects = stubbedEntities.ToResponse() };

            var response = await _classUnderTest.ExecuteAsync(query).ConfigureAwait(false);
            response.Should().BeEquivalentTo(expectedResponse);
            response.ResponseObjects.Should().HaveCount(1);
        }

    }
}
