using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace LbhNotificationsApi.Tests
{
    public class DynamoDbIntegrationTests<TStartup> where TStartup : class
    {
        public HttpClient Client { get; private set; }
        private readonly DynamoDbMockWebApplicationFactory<TStartup> _factory;
        public IDynamoDBContext DynamoDbContext => _factory?.DynamoDbContext;
        protected List<Action> CleanupActions { get; }

        private readonly List<TableDef> _tables = new List<TableDef>
        {

            new TableDef { Name = "Notifications", KeyName = "pk", RangeName="id", KeyType = KeyType.HASH,RangeType = KeyType.RANGE,  KeyScalarType= ScalarAttributeType.S}
        };

        protected DynamoDbIntegrationTests()
        {
            EnsureEnvVarConfigured("DynamoDb_LocalMode", "true");
            EnsureEnvVarConfigured("DynamoDb_LocalServiceUrl", "http://localhost:8000");
            _factory = new DynamoDbMockWebApplicationFactory<TStartup>(_tables);
            Client = _factory.CreateClient();
            CleanupActions = new List<Action>();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private bool _disposed;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing || _disposed) return;
            _factory?.Dispose();
            _disposed = true;
        }
        private static void EnsureEnvVarConfigured(string name, string defaultValue)
        {
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable(name)))
                Environment.SetEnvironmentVariable(name, defaultValue);
        }



    }

    public class TableDef
    {
        public string Name { get; set; }
        public string KeyName { get; set; }
        public string RangeName { get; set; }
        public KeyType KeyType { get; set; }
        public KeyType RangeType { get; set; }
        public ScalarAttributeType KeyScalarType { get; set; }
    }
}
