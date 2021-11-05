using System;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using LbhNotificationsApi.V1.Boundary.Requests;
using LbhNotificationsApi.V1.Domain;
using LbhNotificationsApi.V1.Gateways.Interfaces;
using LbhNotificationsApi.V1.UseCase;
using Moq;
using Xunit;

namespace LbhNotificationsApi.Tests.V1.UseCase
{
    public class AddNotificationUseCaseTest
    {
        private readonly Mock<INotificationGateway> _mockGateway;
        private readonly Mock<INotifyGateway> _mockNotify;
        private readonly AddNotificationUseCase _addUseCase;
        private readonly Fixture _fixture;


        public AddNotificationUseCaseTest()
        {
            _mockGateway = new Mock<INotificationGateway>();
            _mockNotify = new Mock<INotifyGateway>();
            _addUseCase = new AddNotificationUseCase(_mockGateway.Object, _mockNotify.Object);
            _fixture = new Fixture();
        }
        [Fact]
        public async Task AddNotificationReturnSuccessSave()
        {
            var entity = _fixture.Create<NotificationObjectRequest>();
            _mockGateway.Setup(x => x.AddAsync(It.IsAny<Notification>()))
                .Returns(Task.CompletedTask);
            var response = await _addUseCase.ExecuteAsync(entity)
                .ConfigureAwait(false);
            response.Should().NotBe(Guid.Empty);

            _mockGateway.Verify(x => x.AddAsync(It.IsAny<Notification>()), Times.Once);
        }
    }
}
