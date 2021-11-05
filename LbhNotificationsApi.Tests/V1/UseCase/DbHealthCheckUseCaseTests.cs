using System.Threading;
using FluentAssertions;
using LbhNotificationsApi.V1.UseCase;
using Microsoft.Extensions.HealthChecks;
using Moq;
using Xunit;

namespace LbhNotificationsApi.Tests.V1.UseCase
{
    public class DbHealthCheckUseCaseTests
    {
        private readonly DbHealthCheckUseCase _classUnderTest;

        private readonly Bogus.Faker _faker = new Bogus.Faker();
        private readonly string _description;

        public DbHealthCheckUseCaseTests()
        {
            _description = _faker.Random.Words();

            var mockHealthCheckService = new Mock<IHealthCheckService>();
            var compositeHealthCheckResult = new CompositeHealthCheckResult(CheckStatus.Healthy);
            compositeHealthCheckResult.Add("test", CheckStatus.Healthy, _description);


            mockHealthCheckService.Setup(s =>
                    s.CheckHealthAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(compositeHealthCheckResult);

            _classUnderTest = new DbHealthCheckUseCase(mockHealthCheckService.Object);
        }

        [Fact]
        public void ReturnsResponseWithStatus()
        {
            var response = _classUnderTest.Execute();

            response.Should().NotBeNull();
            response.Success.Should().BeTrue();
            response.Message.Should().BeEquivalentTo("test: " + _description);
        }
    }
}
