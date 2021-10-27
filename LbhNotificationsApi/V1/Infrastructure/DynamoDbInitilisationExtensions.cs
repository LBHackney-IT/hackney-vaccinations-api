using System;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Microsoft.Extensions.DependencyInjection;

namespace LbhNotificationsApi.V1.Infrastructure
{
    public static class DynamoDbInitilisationExtensions
    {
        public static void ConfigureDynamoDb(this IServiceCollection services)
        {
            _ = bool.TryParse(Environment.GetEnvironmentVariable("DynamoDb_LocalMode"), out var localMode);

            if (localMode)
            {
                var url = Environment.GetEnvironmentVariable("DynamoDb_LocalServiceUrl");
                services.AddSingleton<IAmazonDynamoDB>(sp =>
                {
                    var clientConfig = new AmazonDynamoDBConfig { ServiceURL = url };
                    return new AmazonDynamoDBClient(clientConfig);
                });
            }
            else
            {
                services.AddAWSService<IAmazonDynamoDB>();
            }

            services.AddScoped<IDynamoDBContext>(sp =>
            {
                var db = sp.GetService<IAmazonDynamoDB>();
                return new DynamoDBContext(db);
            });
        }
    }
}
