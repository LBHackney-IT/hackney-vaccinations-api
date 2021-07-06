using Bogus;
using FluentAssertions;
using LbhNotificationsApi.Tests.TestHelpers;
using LbhNotificationsApi.V1.Controllers.Validators;
using NUnit.Framework;
using ValidationException = LbhNotificationsApi.V1.Controllers.Validators.ValidationException;

namespace LbhNotificationsApi.Tests.V1.Validators
{
    [TestFixture]
    public class EmailRequestValidatorTests
    {
        private EmailRequestValidator _classUnderTest;

        [SetUp]
        public void SetUp()
        {
            _classUnderTest = new EmailRequestValidator();
        }

        [TestCase(TestName = "The email request validator return true for valid email")]
        public void EmailValidatorShouldReturnTrueForValidEmail()
        {
            var email = Faker.Internet.Email(Faker.Name.First());
            var validationResponse = _classUnderTest.ValidateEmail(email);
            validationResponse.Should().BeTrue();
        }

        [TestCase(TestName = "The email request validator throws validation exception if email is empty")]
        public void EmailValidatorShouldThrowExceptionForEmptyEmail()
        {
            var email = "";
            var ex = Assert.Throws<ValidationException>(() => _classUnderTest.ValidateEmail(email));
            ex.Message.Should().Be("Email cannot be blank");
        }

        [TestCase(TestName = "The email request validator throws validation exception if email is not a valid email")]
        public void EmailValidatorShouldThrowExceptionForInvalidEmail()
        {
            var email = Faker.Name.First();
            var ex = Assert.Throws<ValidationException>(() => _classUnderTest.ValidateEmail(email));
            ex.Message.Should().Be("Email invalid");
        }
    }
}
