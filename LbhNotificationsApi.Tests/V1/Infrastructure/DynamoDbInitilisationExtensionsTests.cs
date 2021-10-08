using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using LbhNotificationsApi.V1.Infrastructure;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Xunit;

namespace LbhNotificationsApi.Tests.V1.Infrastructure
{
   
    public class DynamoDbInitilisationExtensionsTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("false")]
        [InlineData("true")]
        public void ConfigureDynamoDbTestNoLocalModeEnvVarUsesAwsService(string localModeEnvVar)
        {
            Environment.SetEnvironmentVariable("DynamoDb_LocalMode", localModeEnvVar);

            var services = new ServiceCollection();
            services.ConfigureDynamoDb();

            services.Any(x => x.ServiceType == typeof(IAmazonDynamoDB)).Should().BeTrue();
            services.Any(x => x.ServiceType == typeof(IDynamoDBContext)).Should().BeTrue();

            Environment.SetEnvironmentVariable("DynamoDb_LocalMode", null);
        }
    }
}
