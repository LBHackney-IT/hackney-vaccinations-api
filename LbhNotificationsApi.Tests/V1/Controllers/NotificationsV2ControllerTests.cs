using System;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using LbhNotificationsApi.Tests.TestHelpers;
using LbhNotificationsApi.V1.Boundary.Requests;
using LbhNotificationsApi.V1.Boundary.Response;
using LbhNotificationsApi.V1.Common.Enums;
using LbhNotificationsApi.V1.Controllers;
using LbhNotificationsApi.V1.UseCase;
using LbhNotificationsApi.V1.UseCase.Interfaces;
using LbhNotificationsApi.V1.Validators.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace LbhNotificationsApi.Tests.V1.Controllers
{
    public class NotificationsV2ControllerTests
    {
        private readonly NotificationsV2Controller _classUnderTest;
        private readonly Mock<ISendSmsNotificationUseCase> _mockSmsNotificationUseCase;
        private readonly Mock<ISendEmailNotificationUseCase> _mockEmailNotificationUseCase;
        private readonly Mock<IGetAllNotificationCase> _getAllNotificationCase;
        private readonly Mock<IGetByIdNotificationCase> _getByIdNotificationCase;
        private readonly Mock<IAddNotificationUseCase> _addNotificationUseCase;
        private readonly Mock<IUpdateNotificationUseCase> _updateNotificationUseCase;
        private readonly Mock<IDeleteNotificationUseCase> _deleteNotification;
        private readonly Mock<IGetAllTemplateCase> _getAllTemplate;


        private readonly Fixture _fixture = new Fixture();

        public NotificationsV2ControllerTests()
        {
            _mockSmsNotificationUseCase = new Mock<ISendSmsNotificationUseCase>();
            _mockEmailNotificationUseCase = new Mock<ISendEmailNotificationUseCase>();
            var mockEmailRequestValidator = new Mock<IEmailRequestValidator>();
            var mockSmsRequestValidator = new Mock<ISmsRequestValidator>();
            _getAllNotificationCase = new Mock<IGetAllNotificationCase>();
            _getByIdNotificationCase = new Mock<IGetByIdNotificationCase>();
            _addNotificationUseCase = new Mock<IAddNotificationUseCase>();
            _updateNotificationUseCase = new Mock<IUpdateNotificationUseCase>();
            _deleteNotification = new Mock<IDeleteNotificationUseCase>();
            _getAllTemplate = new Mock<IGetAllTemplateCase>();
            _classUnderTest = new NotificationsV2Controller(_mockSmsNotificationUseCase.Object,
                _mockEmailNotificationUseCase.Object, mockEmailRequestValidator.Object,
                mockSmsRequestValidator.Object, _getAllNotificationCase.Object, _getByIdNotificationCase.Object,
                _addNotificationUseCase.Object, _updateNotificationUseCase.Object, _deleteNotification.Object, _getAllTemplate.Object);
        }


        [Fact(DisplayName = "Notifications controller sms post request returns response with status")]
        public void SmsControllerCreateReturnsResponseWithStatus()
        {
            var smsNotificationRequest = Fakr.Create<SmsNotificationRequest>();
            var response = _classUnderTest.CreateSmsNotification(smsNotificationRequest) as CreatedResult;
            response?.StatusCode.Should().Be(201);
        }



        [Fact(DisplayName = "Notifications controller email post request returns response with status")]
        public void EmailControllerCreateReturnsResponseWithStatus()
        {
            var emailNotificationRequest = Fakr.Create<EmailNotificationRequest>();
            var response = _classUnderTest.CreateEmailNotification(emailNotificationRequest) as CreatedResult;
            response?.StatusCode.Should().Be(201);
        }


        [Fact(DisplayName = "Notifications controller sms post request calls the validator")]
        public void CreateSmsNotificationCallsValidator()
        {
            var smsNotificationRequest = Fakr.Create<SmsNotificationRequest>();
            _classUnderTest.CreateSmsNotification(smsNotificationRequest);
            _mockSmsNotificationUseCase.Verify(u => u.Execute(It.IsAny<SmsNotificationRequest>()), Times.Once);
        }


        [Fact(DisplayName = "Notifications controller sms post request calls the send confirmation use case")]
        public void CreateEmailNotificationCallsValidator()
        {
            var emailNotificationRequest = Fakr.Create<EmailNotificationRequest>();
            emailNotificationRequest.Email = Faker.Internet.Email(Faker.Name.First());
            _classUnderTest.CreateEmailNotification(emailNotificationRequest);
            _mockEmailNotificationUseCase.Verify(u => u.Execute(It.IsAny<EmailNotificationRequest>()), Times.Once);
        }

        [Fact(DisplayName = "Notifications controller sms post request calls the send confirmation use case")]
        public void CallsSendSmsNotificationUseCase()
        {
            var smsNotificationRequest = Fakr.Create<SmsNotificationRequest>();
            _classUnderTest.CreateSmsNotification(smsNotificationRequest);
            _mockSmsNotificationUseCase.Verify(u => u.Execute(It.IsAny<SmsNotificationRequest>()), Times.Once);
        }


        [Fact(DisplayName = "Notifications controller sms post request calls the send confirmation use case")]
        public void CallsSendEmailNotificationUseCase()
        {
            var emailNotificationRequest = Fakr.Create<EmailNotificationRequest>();
            _classUnderTest.CreateEmailNotification(emailNotificationRequest);
            _mockEmailNotificationUseCase.Verify(u => u.Execute(It.IsAny<EmailNotificationRequest>()), Times.Once);
        }

        [Fact]
        public async Task GetAllNotificationReturns200()
        {
            // Arrange
            var notifications = _fixture.Create<NotificationResponseObjectList>();
            var query = new NotificationSearchQuery();
            _getAllNotificationCase.Setup(x => x.ExecuteAsync(query)).ReturnsAsync(notifications);

            // Act
            var response = await _classUnderTest.ListNotificationAsync(query).ConfigureAwait(false);

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

            _addNotificationUseCase.Setup(x => x.ExecuteAsync(It.IsAny<NotificationObjectRequest>()))
                .ReturnsAsync(It.IsAny<Guid>());

            // Act
            var response = await _classUnderTest.AddAsync(new NotificationObjectRequest()).ConfigureAwait(false);

            // Assert
            response.Should().BeOfType(typeof(CreatedResult));
            (response as CreatedResult)?.Value.Should().NotBeNull();
        }

        [Fact]
        public void PostNewPersonIdAsyncExceptionIsThrown()
        {
            // Arrange
            var exception = new ApplicationException("Test exception");
            _addNotificationUseCase.Setup(x => x.ExecuteAsync(It.IsAny<NotificationObjectRequest>()))
                                 .ThrowsAsync(exception);

            // Act
            Func<Task<IActionResult>> func = async () => await _classUnderTest.AddAsync(new NotificationObjectRequest())
                .ConfigureAwait(false);

            // Assert
            func.Should().Throw<ApplicationException>().WithMessage(exception.Message);
        }

        [Fact]
        public async Task UpdateNotificationAsyncNotFoundReturnsNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            var request = new UpdateRequest
            {
                ActionType = ActionType.Initiated,
                ActionNote = null
            };
            _updateNotificationUseCase.Setup(x => x.ExecuteAsync(id, request)).ReturnsAsync((ActionResponse) null);
            // Act
            var response = await _classUnderTest.UpdateAsync(id, request).ConfigureAwait(false);

            // Assert
            response.Should().BeOfType(typeof(NotFoundObjectResult));
            (response as NotFoundObjectResult)?.Value.Should().BeEquivalentTo(id);
        }

        [Fact]
        public async Task UpdateNotificationAsyncReturnsSuccess()
        {
            // Arrange
            var id = Guid.NewGuid();

            var request = new UpdateRequest { ActionNote = "", ActionType = ActionType.Approved };
            var updateResponse = new ActionResponse { Status = true };
            _getByIdNotificationCase.Setup(x => x.ExecuteAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new NotificationResponseObject() { PerformedActionType = ActionType.Initiated.ToString() });
            _updateNotificationUseCase.Setup(x => x.ExecuteAsync(id, request)).ReturnsAsync(updateResponse);

            // Act
            var response = await _classUnderTest.UpdateAsync(id, request).ConfigureAwait(false);
            // Assert
            response.Should().BeOfType(typeof(OkObjectResult));
            (response as OkObjectResult)?.Value.Should().BeEquivalentTo(updateResponse);

        }

        [Fact]
        public async Task UpdateNotificationAsyncReturnsFailed()
        {
            // Arrange
            var id = Guid.NewGuid();
            var request = new UpdateRequest { ActionNote = "", ActionType = ActionType.Approved };
            var updateResponse = new ActionResponse { Status = false };
            _getByIdNotificationCase.Setup(x => x.ExecuteAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new NotificationResponseObject() { Id = id, PerformedActionType = ActionType.Initiated.ToString() });
            _updateNotificationUseCase.Setup(x => x.ExecuteAsync(id, request)).ReturnsAsync(updateResponse);

            // Act
            var response = await _classUnderTest.UpdateAsync(id, request).ConfigureAwait(false);
            // Assert
            response.Should().BeOfType(typeof(BadRequestObjectResult));
            (response as BadRequestObjectResult)?.Value.Should().BeEquivalentTo(updateResponse);

        }

        [Fact]
        public void UpdateNotificationAsyncExceptionIsThrown()
        {
            // Arrange
            var id = Guid.NewGuid();
            var exception = new ApplicationException("Test exception");
            _getByIdNotificationCase.Setup(x => x.ExecuteAsync(It.IsAny<Guid>()))
               .ReturnsAsync(new NotificationResponseObject() { Id = id, PerformedActionType = ActionType.Initiated.ToString() });
            _updateNotificationUseCase.Setup(x => x.ExecuteAsync(id, It.IsAny<UpdateRequest>())).ThrowsAsync(exception);

            // Act
            Func<Task<IActionResult>> func = async () => await _classUnderTest.UpdateAsync(id, new UpdateRequest())
                .ConfigureAwait(false);
            // Assert
            func.Should().Throw<ApplicationException>().WithMessage(exception.Message);
        }
    }
}
