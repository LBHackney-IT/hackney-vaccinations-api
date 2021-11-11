using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using NotificationsApi.V1.Infrastructure;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using LbhNotificationsApi.V1.Infrastructure;
using Xunit;
using Hackney.Core.DynamoDb;

namespace NotificationsApi.Tests.V1.Infrastructure
{

    public class DynamoDbInitialisationExtensionsTests
    {

        [Theory]
        [InlineData(null)]
        [InlineData("false")]
        [InlineData("true")]
        public void ConfigureDynamoDbTestNoLocalModeEnvVarUsesAwsService(string localModeEnvVar)
        {
            Environment.SetEnvironmentVariable("DynamoDb_LocalMode", localModeEnvVar);

            var services = new ServiceCollection();
            services.ConfigureDynamoDB();
            services.Any(x => x.ServiceType == typeof(IAmazonDynamoDB)).Should().BeTrue();
            services.Any(x => x.ServiceType == typeof(IDynamoDBContext)).Should().BeTrue();

            Environment.SetEnvironmentVariable("DynamoDb_LocalMode", null);
        }
    }
}
