using AutoFixture;
using FluentAssertions;
using LbhNotificationsApi.V1.Common.Enums;
using LbhNotificationsApi.V1.Domain;
using LbhNotificationsApi.V1.Gateways.Interfaces;
using LbhNotificationsApi.V1.UseCase;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace LbhNotificationsApi.Tests.V1.UseCase
{
    public class DeleteNotificationUseCaseTest
    {
        private readonly Mock<INotificationGateway> _mockGateway;
        private readonly DeleteNotificationUseCase _classUnderTest;
        private readonly Fixture _fixture;


        public DeleteNotificationUseCaseTest()
        {
            _mockGateway = new Mock<INotificationGateway>();
            _classUnderTest = new DeleteNotificationUseCase(_mockGateway.Object);
            _fixture = new Fixture();
        }

        [Fact]
        public async Task DeleteNotificationUseCaseReturnsTrueResult()
        {
            var id = Guid.NewGuid();

            _mockGateway.Setup(x => x.DeleteAsync(id)).ReturnsAsync(1);


            var response = await _classUnderTest.ExecuteAsync(id)
                .ConfigureAwait(false);
            response.Status.Should().BeTrue();
            response.Message.Should().BeEquivalentTo("successfully removed");

        }



        [Fact]
        public void UpdateNotificationAsyncExceptionIsThrown()
        {
            // Arrange
            var id = Guid.NewGuid();
            var exception = new NullReferenceException($"Record with Id={id} not found");
            _mockGateway.Setup(x => x.DeleteAsync(id)).ThrowsAsync(exception);

            // Act
            Func<Task> func = async () =>
                await _classUnderTest.ExecuteAsync(id).ConfigureAwait(false);

            // Assert
            func.Should().Throw<NullReferenceException>().WithMessage(exception.Message);
        }
    }
}
