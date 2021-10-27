using FluentAssertions;
using LbhNotificationsApi.Tests.TestHelpers;
using LbhNotificationsApi.V1.Boundary.Requests;
using LbhNotificationsApi.V1.Controllers;
using LbhNotificationsApi.V1.UseCase.Interfaces;
using LbhNotificationsApi.V1.Validators.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace LbhNotificationsApi.Tests.V1.Controllers
{


    public class NotificationsControllerTests
    {
        private readonly NotificationsController _classUnderTest;
        private readonly Mock<ISendSmsNotificationUseCase> _mockSmsNotificationUseCase;
        private readonly Mock<ISendEmailNotificationUseCase> _mockEmailNotificationUseCase;

        public NotificationsControllerTests()
        {
            _mockSmsNotificationUseCase = new Mock<ISendSmsNotificationUseCase>();
            _mockEmailNotificationUseCase = new Mock<ISendEmailNotificationUseCase>();
            var mockEmailRequestValidator = new Mock<IEmailRequestValidator>();
            var mockSmsRequestValidator = new Mock<ISmsRequestValidator>();
            _classUnderTest = new NotificationsController(_mockSmsNotificationUseCase.Object,
                _mockEmailNotificationUseCase.Object, mockEmailRequestValidator.Object,
                mockSmsRequestValidator.Object);
        }


        [Fact(DisplayName = "Notifications controller sms post request returns response with status")]
        public void SmsControllerCreateReturnsResponseWithStatus()
        {
            var smsNotificationRequest = Fakr.Create<SmsNotificationRequest>();
            var response = _classUnderTest.CreateSMSNotification(smsNotificationRequest) as CreatedResult;
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
            _classUnderTest.CreateSMSNotification(smsNotificationRequest);
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
            _classUnderTest.CreateSMSNotification(smsNotificationRequest);
            _mockSmsNotificationUseCase.Verify(u => u.Execute(It.IsAny<SmsNotificationRequest>()), Times.Once);
        }


        [Fact(DisplayName = "Notifications controller sms post request calls the send confirmation use case")]
        public void CallsSendEmailNotificationUseCase()
        {
            var emailNotificationRequest = Fakr.Create<EmailNotificationRequest>();
            _classUnderTest.CreateEmailNotification(emailNotificationRequest);
            _mockEmailNotificationUseCase.Verify(u => u.Execute(It.IsAny<EmailNotificationRequest>()), Times.Once);
        }

    }
}
