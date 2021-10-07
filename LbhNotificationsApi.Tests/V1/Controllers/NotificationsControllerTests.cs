using FluentAssertions;
using LbhNotificationsApi.Tests.TestHelpers;
using LbhNotificationsApi.V1.Boundary.Requests;
using LbhNotificationsApi.V1.Controllers;
using LbhNotificationsApi.V1.UseCase.Interfaces;
using LbhNotificationsApi.V1.Validators.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace LbhNotificationsApi.Tests.V1.Controllers
{

    [TestFixture]
    public class NotificationsControllerTests
    {
        private NotificationsController _classUnderTest;
        private Mock<ISendSmsNotificationUseCase> _mockSmsNotificationUseCase;
        private Mock<ISendEmailNotificationUseCase> _mockEmailNotificationUseCase;
        private Mock<IEmailRequestValidator> _mockEmailRequestValidator;
        private Mock<ISmsRequestValidator> _mockSmsRequestValidator;
        private Mock<IGetAllNotificationCase> _getAllNotificationCase;
        private Mock<IGetByIdNotificationCase> _getByIdNotificationCase;
        private Mock<IAddNotificationUseCase> _addNotificationUseCase;
        private Mock<IUpdateNotificationUseCase> _updateNotificationUseCase;

        [SetUp]
        public void SetUp()
        {
            _mockEmailNotificationUseCase = new Mock<ISendEmailNotificationUseCase>();
            _mockSmsNotificationUseCase = new Mock<ISendSmsNotificationUseCase>();
            _mockEmailRequestValidator = new Mock<IEmailRequestValidator>();
            _mockSmsRequestValidator = new Mock<ISmsRequestValidator>();
            _getAllNotificationCase = new Mock<IGetAllNotificationCase>();
            _getByIdNotificationCase = new Mock<IGetByIdNotificationCase>();
            _addNotificationUseCase = new Mock<IAddNotificationUseCase>();
            _updateNotificationUseCase = new Mock<IUpdateNotificationUseCase>();
            _classUnderTest = new NotificationsController(
                _mockSmsNotificationUseCase.Object,
                _mockEmailNotificationUseCase.Object,
                _mockEmailRequestValidator.Object,
                _mockSmsRequestValidator.Object,
                _getAllNotificationCase.Object,
                _getByIdNotificationCase.Object,
                _addNotificationUseCase.Object,
                _updateNotificationUseCase.Object);
        }

        [TestCase(TestName = "Notifications controller sms post request returns response with status")]
        public void SmsControllerCreateReturnsResponseWithStatus()
        {
            var smsNotificationRequest = Fakr.Create<SmsNotificationRequest>();
            var response = _classUnderTest.CreateSmsNotification(smsNotificationRequest) as CreatedResult;
            response?.StatusCode.Should().Be(201);
        }


        [TestCase(TestName = "Notifications controller email post request returns response with status")]
        public void EmailControllerCreateReturnsResponseWithStatus()
        {
            var emailNotificationRequest = Fakr.Create<EmailNotificationRequest>();
            var response = _classUnderTest.CreateEmailNotification(emailNotificationRequest) as CreatedResult;
            response?.StatusCode.Should().Be(201);
        }

        [TestCase(TestName = "Notifications controller sms post request calls the validator")]
        public void CreateSmsNotificationCallsValidator()
        {
            var smsNotificationRequest = Fakr.Create<SmsNotificationRequest>();
            _classUnderTest.CreateSmsNotification(smsNotificationRequest);
            _mockSmsNotificationUseCase.Verify(u => u.Execute(It.IsAny<SmsNotificationRequest>()), Times.Once);
        }

        [TestCase(TestName = "Notifications controller email post request calls the validator")]
        public void CreateEmailNotificationCallsValidator()
        {
            var emailNotificationRequest = Fakr.Create<EmailNotificationRequest>();
            emailNotificationRequest.Email = Faker.Internet.Email(Faker.Name.First());
            _classUnderTest.CreateEmailNotification(emailNotificationRequest);
            _mockEmailNotificationUseCase.Verify(u => u.Execute(It.IsAny<EmailNotificationRequest>()), Times.Once);
        }

        [TestCase(TestName = "Notifications controller sms post request calls the send confirmation use case")]
        public void CallsSendSmsNotificationUseCase()
        {
            var smsNotificationRequest = Fakr.Create<SmsNotificationRequest>();
            _classUnderTest.CreateSmsNotification(smsNotificationRequest);
            _mockSmsNotificationUseCase.Verify(u => u.Execute(It.IsAny<SmsNotificationRequest>()), Times.Once);
        }

        [TestCase(TestName = "Notifications controller sms post request calls the send confirmation use case")]
        public void CallsSendEmailNotificationUseCase()
        {
            var emailNotificationRequest = Fakr.Create<EmailNotificationRequest>();
            _classUnderTest.CreateEmailNotification(emailNotificationRequest);
            _mockEmailNotificationUseCase.Verify(u => u.Execute(It.IsAny<EmailNotificationRequest>()), Times.Once);
        }

    }
}
