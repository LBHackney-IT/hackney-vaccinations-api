using System.Threading.Tasks;
using FluentAssertions;
using LbhNotificationsApi.V1;
using LbhNotificationsApi.V1.Controllers;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace LbhNotificationsApi.Tests.V1.Infrastructure
{
    public class CorrelationMiddlewareTest
    {
        private readonly CorrelationMiddleware _sut;


        public CorrelationMiddlewareTest()
        {
            _sut = new CorrelationMiddleware(null);
        }

        [Fact]
        public async Task DoesNotReplaceCorrelationIdIfOneExists()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            const string headerValue = "123";

            httpContext.HttpContext.Request.Headers.Add(Constants.CorrelationId, headerValue);

            // Act
            await _sut.InvokeAsync(httpContext).ConfigureAwait(false);

            // Assert
            httpContext.HttpContext.Request.Headers[Constants.CorrelationId].Should().BeEquivalentTo(headerValue);
        }

        [Fact]
        public async Task AddsCorrelationIdIfOneDoesNotExist()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();

            // Act
            await _sut.InvokeAsync(httpContext).ConfigureAwait(false);

            // Assert
            httpContext.HttpContext.Request.Headers[Constants.CorrelationId].Should().HaveCountGreaterThan(0);
        }
    }
}
