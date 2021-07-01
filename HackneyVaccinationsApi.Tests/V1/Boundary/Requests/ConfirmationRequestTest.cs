using Bogus;
using FluentAssertions;
using HackneyVaccinationsApi.Tests.TestHelpers;
using HackneyVaccinationsApi.V1.Boundary.Requests;
using NUnit.Framework;

namespace HackneyVaccinationsApi.Tests.V1.Boundary.Requests
{
    [TestFixture]
    public class ConfirmationRequestTest
    {
        [TestCase(TestName = "The confirmation request should have the correct properties")]
        public void ConfirmationRequestShouldHaveCorrectProperties()
        {
            var entityType = typeof(ConfirmationRequest);
            entityType.GetProperties().Length.Should().Be(3);
            var entity = Fakr.Create<ConfirmationRequest>();
            Assert.That(entity, Has.Property("Email").InstanceOf(typeof(string)));
            Assert.That(entity, Has.Property("MobileNumber").InstanceOf(typeof(string)));
            Assert.That(entity, Has.Property("BookingSlot").InstanceOf(typeof(string)));
        }
    }
}
