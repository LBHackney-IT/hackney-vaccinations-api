using System;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using AutoFixture;
using FluentAssertions;
using LbhNotificationsApi.V1.Boundary.Requests;
using LbhNotificationsApi.V1.Boundary.Response;
using LbhNotificationsApi.V1.Common.Enums;
using LbhNotificationsApi.V1.Domain;
using LbhNotificationsApi.V1.Gateways.Interfaces;
using LbhNotificationsApi.V1.UseCase;
using Moq;
using Xunit;

namespace LbhNotificationsApi.Tests.V1.UseCase
{
    public class UpdateNotificationUseCaseTest
    {
        private readonly Mock<IDynamoDBContext> _dynamoDb;
        private readonly Mock<INotificationGateway> _mockGateway;
        private readonly UpdateNotificationUseCase _updateUseCase;
        private readonly Fixture _fixture;


        public UpdateNotificationUseCaseTest()
        {
            _dynamoDb = new Mock<IDynamoDBContext>();
            _mockGateway = new Mock<INotificationGateway>();
            _updateUseCase = new UpdateNotificationUseCase(_mockGateway.Object);
            _fixture = new Fixture();
        }
        [Fact]
        public async Task UpdateNotificationUseCaseReturnsTrueResult()
        {
            var id = Guid.NewGuid();
            var entity = _fixture.Build<Notification>()
                                .With(_ => _.Id, id)
                                .With(_ => _.PerformedActionDate, DateTime.UtcNow)
                                .With(_ => _.PerformedActionType, ActionType.Approved)
                                .Create();
            var request = _fixture.Build<UpdateRequest>()
                                 .With(_ => _.ActionType, ActionType.Approved)
                                 .Create();
            _mockGateway.Setup(x => x.UpdateAsync(id, request)).ReturnsAsync(entity);


            var response = await _updateUseCase.ExecuteAsync(id, request)
                .ConfigureAwait(false);
            response.Status.Should().BeTrue();
            response.Message.Should().BeEquivalentTo("action was successfully");

        }

        [Fact]
        public async Task UpdateNotificationUseCaseReturnsFalseResult()
        {
            var id = Guid.NewGuid();

            var request = _fixture.Build<UpdateRequest>()
                                 .With(_ => _.ActionType, ActionType.Approved)
                                 .Create();
            _mockGateway.Setup(x => x.UpdateAsync(id, request)).ReturnsAsync((Notification) null);
            var response = await _updateUseCase.ExecuteAsync(id, request).ConfigureAwait(false);
            response.Status.Should().BeFalse();
            response.Message.Should().BeEquivalentTo("action failed");

        }

        [Fact]
        public void UpdateNotificationAsyncExceptionIsThrown()
        {
            // Arrange
            var request = _fixture.Create<UpdateRequest>();
            var query = Guid.NewGuid();
            var exception = new ApplicationException("Test exception");
            _mockGateway.Setup(x => x.UpdateAsync(query, request)).ThrowsAsync(exception);

            // Act
            Func<Task<ActionResponse>> func = async () =>
                await _updateUseCase.ExecuteAsync(query, request).ConfigureAwait(false);

            // Assert
            func.Should().Throw<ApplicationException>().WithMessage(exception.Message);
        }
    }
}
