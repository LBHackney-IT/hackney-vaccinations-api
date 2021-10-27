using LbhNotificationsApi.V1.UseCase;
using FluentAssertions;
using Xunit;


namespace LbhNotificationsApi.Tests.V1.UseCase
{

    public class ThrowOpsErrorUsecaseTests
    {
        [Fact]
        public void ThrowsTestOpsErrorException()
        {
            var ex = Assert.Throws<TestOpsErrorException>(
                ThrowOpsErrorUsecase.Execute);

            const string expected = "This is a test exception to test our integrations";

            ex.Message.Should().BeEquivalentTo(expected);
        }
    }
}
