using Bogus;
using FluentAssertions;
using LbhNotificationsApi.Tests.TestHelpers;
using LbhNotificationsApi.V1.Controllers.Validators;
using NUnit.Framework;
using ValidationException = LbhNotificationsApi.V1.Controllers.Validators.ValidationException;

namespace LbhNotificationsApi.Tests.V1.Validators
{
    [TestFixture]
    public class SmsRequestValidatorTests
    {
        private SmsRequestValidator _classUnderTest;

        [SetUp]
        public void SetUp()
        {
            _classUnderTest = new SmsRequestValidator();
        }

        [TestCase(TestName = "The sms request validator return true for valid mobile number")]
        public void SmsValidatorShouldReturnTrueForValidMobileNumber()
        {
            var phone = Faker.Phone.Number();
            var validationResponse = _classUnderTest.ValidateSms(phone);
            validationResponse.Should().BeTrue();
        }

        [TestCase(TestName = "The sms request validator throws validation exception if mobile number is empty")]
        public void EmailValidatorShouldThrowExceptionForEmptyEmail()
        {
            var phone = "";
            var ex = Assert.Throws<ValidationException>(() => _classUnderTest.ValidateSms(phone));
            ex.Message.Should().Be("Mobile number cannot be blank");
        }
    }
}
