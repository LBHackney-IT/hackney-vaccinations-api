using LbhNotificationsApi.V1.Gateways.Interfaces;
using LbhNotificationsApi.V1.UseCase;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace LbhNotificationsApi.Tests.V1.UseCase
{
    public class GetAllTemplateCaseTest
    {
        private readonly Mock<INotifyGateway> _mockGateway;
        private readonly GetAllTemplateCase _classUnderTest;

        public GetAllTemplateCaseTest()
        {
            _mockGateway = new Mock<INotifyGateway>();
            _classUnderTest = new GetAllTemplateCase(_mockGateway.Object);
        }

        [Fact]
        public async Task UseCaseCallsGatewayGetAllTemplate()
        {
            var request = Guid.NewGuid().ToString();
            await _classUnderTest.ExecuteAsync(request).ConfigureAwait(false);
            _mockGateway.Verify(gw => gw.GetTaskAllTemplateAsync(It.IsAny<string>()), Times.Once);
        }
    }
}
