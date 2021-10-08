using FluentAssertions;
using LbhNotificationsApi.Tests.TestHelpers;
using LbhNotificationsApi.V1.Boundary.Requests;
using LbhNotificationsApi.V1.Controllers.Validators;
using Xunit;
using ValidationException = LbhNotificationsApi.V1.Validators.ValidationException;

namespace LbhNotificationsApi.Tests.V1.Validators
{

    public class SmsRequestValidatorTests
    {
        private readonly SmsRequestValidator _classUnderTest;

        public SmsRequestValidatorTests()
        {
            _classUnderTest = new SmsRequestValidator();
        }
        [Fact(DisplayName = "The sms request validator return true for valid mobile number")]
        public void SmsValidatorShouldReturnTrueForValidMobileNumber()
        {
            var request = Fakr.Create<SmsNotificationRequest>();
            var validationResponse = _classUnderTest.ValidateSmsRequest(request);
            validationResponse.Should().BeTrue();
        }

        [Fact(DisplayName = "The sms request validator throws validation exception if mobile number is empty")]
        public void SmsValidatorShouldThrowExceptionForEmptyEmail()
        {
            var request = Fakr.Create<SmsNotificationRequest>();
            request.MobileNumber = "";
            var ex = Assert.Throws<ValidationException>(() => _classUnderTest.ValidateSmsRequest(request));
            ex.Message.Should().Be("Mobile number cannot be blank");
        }

        [Fact(DisplayName = "The sms request validator throws validation exception if a service key is not provided")]
        public void SmsValidatorShouldThrowExceptionForEmptyServiceKey()
        {
            var request = Fakr.Create<SmsNotificationRequest>();
            request.ServiceKey = "";
            var ex = Assert.Throws<ValidationException>(() => _classUnderTest.ValidateSmsRequest(request));
            ex.Message.Should().Be("A service key must be provided");
        }

        [Fact(DisplayName = "The sms request validator throws validation exception if mobile number is empty")]
        public void SmsValidatorShouldThrowExceptionForEmptyTemplateId()
        {
            var request = Fakr.Create<SmsNotificationRequest>();
            request.TemplateId = "";
            var ex = Assert.Throws<ValidationException>(() => _classUnderTest.ValidateSmsRequest(request));
            ex.Message.Should().Be("A template id must be provided");
        }
    }
}
