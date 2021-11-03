using FluentAssertions;
using LbhNotificationsApi.Tests.TestHelpers;
using LbhNotificationsApi.V1.Boundary.Requests;
using LbhNotificationsApi.V1.Validators;
using Xunit;


namespace LbhNotificationsApi.Tests.V1.Validators
{

    public class EmailRequestValidatorTests
    {
        private readonly EmailRequestValidator _classUnderTest;

        public EmailRequestValidatorTests()
        {
            _classUnderTest = new EmailRequestValidator();
        }
        [Fact(DisplayName = "The email request validator return true for valid email")]
        public void EmailValidatorShouldReturnTrueForValidEmail()
        {
            var request = Fakr.Create<EmailNotificationRequest>();
            request.Email = "user.example.com"; //Faker.Internet.Email();
            var validationResponse = _classUnderTest.ValidateEmailRequest(request);
            validationResponse.Should().BeTrue();
        }

        [Fact(DisplayName = "The email request validator throws validation exception if email is empty")]
        public void EmailValidatorShouldThrowExceptionForEmptyEmail()
        {
            var request = Fakr.Create<EmailNotificationRequest>();
            request.Email = "";
            var ex = Assert.Throws<ValidationException>(() => _classUnderTest.ValidateEmailRequest(request));
            ex.Message.Should().Be("Email cannot be blank");
        }

        [Fact(DisplayName = "The email request validator throws validation exception if email is not a valid email")]
        public void EmailValidatorShouldThrowExceptionForInvalidEmail()
        {
            var request = Fakr.Create<EmailNotificationRequest>();
            request.Email = Faker.Name.First();
            var ex = Assert.Throws<ValidationException>(() => _classUnderTest.ValidateEmailRequest(request));
            ex.Message.Should().Be("Email invalid");
        }

        [Fact(DisplayName = "The email request validator throws validation exception if a service key is not provided")]
        public void SmsValidatorShouldThrowExceptionForEmptyServiceKey()
        {
            var request = Fakr.Create<EmailNotificationRequest>();
            request.ServiceKey = "";
            var ex = Assert.Throws<ValidationException>(() => _classUnderTest.ValidateEmailRequest(request));
            ex.Message.Should().Be("A service key must be provided");
        }

        [Fact(DisplayName = "The email request validator throws validation exception if mobile number is empty")]
        public void SmsValidatorShouldThrowExceptionForEmptyTemplateId()
        {
            var request = Fakr.Create<EmailNotificationRequest>();
            request.TemplateId = "";
            var ex = Assert.Throws<ValidationException>(() => _classUnderTest.ValidateEmailRequest(request));
            ex.Message.Should().Be("A template id must be provided");
        }
    }
}
