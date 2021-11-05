using System;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using LbhNotificationsApi.V1.Boundary.Requests;
using LbhNotificationsApi.V1.Boundary.Response;
using LbhNotificationsApi.V1.Controllers;
using LbhNotificationsApi.V1.UseCase.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace LbhNotificationsApi.Tests.V1.Controllers
{
    public class NotificationsControllerTest
    {
        private readonly Mock<IGetAllNotificationCase> _getAllNotificationCase;
        private readonly Mock<IGetByIdNotificationCase> _getByIdNotificationCase;
        private readonly Mock<IAddNotificationUseCase> _addNotificationUseCase;
        private readonly Mock<IUpdateNotificationUseCase> _updateNotificationUseCase;
        private readonly NotificationsV2Controller _classUnderTest;
        private readonly Fixture _fixture = new Fixture();
        public NotificationsControllerTest()
        {
            _getAllNotificationCase = new Mock<IGetAllNotificationCase>();
            _getByIdNotificationCase = new Mock<IGetByIdNotificationCase>();
            _addNotificationUseCase = new Mock<IAddNotificationUseCase>();
            _updateNotificationUseCase = new Mock<IUpdateNotificationUseCase>();
            _classUnderTest = new NotificationsV2Controller(_getAllNotificationCase.Object, _getByIdNotificationCase.Object, _addNotificationUseCase.Object, _updateNotificationUseCase.Object);
        }

        [Fact]
        public async Task GetAllNotificationReturns200()
        {
            // Arrange
            var notifications = _fixture.Create<NotificationResponseObjectList>();
            _getAllNotificationCase.Setup(x => x.ExecuteAsync()).ReturnsAsync(notifications);

            // Act
            var response = await _classUnderTest.ListNotificationAsync().ConfigureAwait(false);

            // Assert
            response.Should().BeOfType(typeof(OkObjectResult));
            (response as OkObjectResult)?.Value.Should().Be(notifications);
        }

        [Fact]
        public async Task GetNotificationByTargetIdAsyncReturnsNotFound()
        {
            // Arrange
            var targetId = Guid.NewGuid();
            _getByIdNotificationCase.Setup(x => x.ExecuteAsync(targetId)).ReturnsAsync((NotificationResponseObject) null);
            // Act
            var response = await _classUnderTest.GetAsync(targetId).ConfigureAwait(false);

            // Assert
            response.Should().BeOfType(typeof(NotFoundObjectResult));
            (response as NotFoundObjectResult)?.Value.Should().Be(targetId);
        }

        [Fact]
        public async Task GetNotificationByTargetIdAsyncReturns200Response()
        {
            // Arrange
            var targetId = Guid.NewGuid();
            var noticationResponse = _fixture.Create<NotificationResponseObject>();
            _getByIdNotificationCase.Setup(x => x.ExecuteAsync(targetId)).ReturnsAsync(noticationResponse);

            // Act
            var response = await _classUnderTest.GetAsync(targetId).ConfigureAwait(false);

            // Assert
            response.Should().BeOfType(typeof(OkObjectResult));
            (response as OkObjectResult)?.Value.Should().BeEquivalentTo(noticationResponse);
        }

        [Fact]
        public void GetNotificationByTargetIdAsyncExceptionIsThrown()
        {
            // Arrange
            var targetId = Guid.NewGuid();
            var exception = new ApplicationException("Test exception");
            _getByIdNotificationCase.Setup(x => x.ExecuteAsync(targetId)).ThrowsAsync(exception);

            // Act
            Func<Task<IActionResult>> func = async () => await _classUnderTest.GetAsync(targetId).ConfigureAwait(false);

            // Assert
            func.Should().Throw<ApplicationException>().WithMessage(exception.Message);
        }

        [Fact]
        public async Task PostNewNotificationAsyncReturnsSuccessResponse()
        {
            // Arrange
            var targetId = Guid.Parse("10ec608c-f2f3-4676-bbd3-3b20962aafe6");
            _addNotificationUseCase.Setup(x => x.ExecuteAsync(It.IsAny<NotificationRequest>()))
                .ReturnsAsync(targetId);

            // Act
            var response = await _classUnderTest.AddAsync(new NotificationRequest()).ConfigureAwait(false);

            // Assert
            response.Should().BeOfType(typeof(CreatedAtActionResult));
            (response as CreatedAtActionResult)?.Value.Should().BeEquivalentTo(new { targetId });
        }

        [Fact]
        public void PostNewPersonIdAsyncExceptionIsThrown()
        {
            // Arrange
            var exception = new ApplicationException("Test exception");
            _addNotificationUseCase.Setup(x => x.ExecuteAsync(It.IsAny<NotificationRequest>()))
                                 .ThrowsAsync(exception);

            // Act
            Func<Task<IActionResult>> func = async () => await _classUnderTest.AddAsync(new NotificationRequest())
                .ConfigureAwait(false);

            // Assert
            func.Should().Throw<ApplicationException>().WithMessage(exception.Message);
        }

        [Fact]
        public async Task UpdateNotificationAsyncNotFoundReturnsNotFound()
        {
            // Arrange
            var targetId = Guid.NewGuid();
            var request = new ApprovalRequest
            {
                ApprovalStatus = ApprovalStatus.Initiated,
                ApprovalNote = null
            };
            _updateNotificationUseCase.Setup(x => x.ExecuteAsync(targetId, request)).ReturnsAsync((ActionResponse) null);
            // Act
            var response = await _classUnderTest.UpdateAsync(targetId, request).ConfigureAwait(false);

            // Assert
            response.Should().BeOfType(typeof(NotFoundObjectResult));
            (response as NotFoundObjectResult)?.Value.Should().BeEquivalentTo(targetId);
        }

        [Fact]
        public async Task UpdateNotificationAsyncReturnsSuccess()
        {
            // Arrange
            var targetId = Guid.NewGuid();

            var request = new ApprovalRequest { ApprovalNote = "", ApprovalStatus = ApprovalStatus.Approved };
            var updateResponse = new ActionResponse { Status = true };
            _getByIdNotificationCase.Setup(x => x.ExecuteAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new NotificationResponseObject() { TargetId = targetId, ApprovalStatus = ApprovalStatus.Initiated });
            _updateNotificationUseCase.Setup(x => x.ExecuteAsync(targetId, request)).ReturnsAsync(updateResponse);

            // Act
            var response = await _classUnderTest.UpdateAsync(targetId, request).ConfigureAwait(false);
            // Assert
            response.Should().BeOfType(typeof(OkObjectResult));
            (response as OkObjectResult)?.Value.Should().BeEquivalentTo(updateResponse);

        }

        [Fact]
        public async Task UpdateNotificationAsyncReturnsFailed()
        {
            // Arrange
            var targetId = Guid.NewGuid();
            var request = new ApprovalRequest { ApprovalNote = "", ApprovalStatus = ApprovalStatus.Approved };
            var updateResponse = new ActionResponse { Status = false };
            _getByIdNotificationCase.Setup(x => x.ExecuteAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new NotificationResponseObject() { TargetId = targetId, ApprovalStatus = ApprovalStatus.Initiated });
            _updateNotificationUseCase.Setup(x => x.ExecuteAsync(targetId, request)).ReturnsAsync(updateResponse);

            // Act
            var response = await _classUnderTest.UpdateAsync(targetId, request).ConfigureAwait(false);
            // Assert
            response.Should().BeOfType(typeof(BadRequestObjectResult));
            (response as BadRequestObjectResult)?.Value.Should().BeEquivalentTo(updateResponse);

        }

        [Fact]
        public void UpdateNotificationAsyncExceptionIsThrown()
        {
            // Arrange
            var targetId = Guid.NewGuid();
            var exception = new ApplicationException("Test exception");
            _getByIdNotificationCase.Setup(x => x.ExecuteAsync(It.IsAny<Guid>()))
               .ReturnsAsync(new NotificationResponseObject() { TargetId = targetId, ApprovalStatus = ApprovalStatus.Initiated });
            _updateNotificationUseCase.Setup(x => x.ExecuteAsync(targetId, It.IsAny<ApprovalRequest>())).ThrowsAsync(exception);

            // Act
            Func<Task<IActionResult>> func = async () => await _classUnderTest.UpdateAsync(targetId, new ApprovalRequest())
                .ConfigureAwait(false);
            // Assert
            func.Should().Throw<ApplicationException>().WithMessage(exception.Message);
        }
    }
}
