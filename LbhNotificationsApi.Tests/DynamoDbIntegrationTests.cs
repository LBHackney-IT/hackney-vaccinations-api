using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

namespace LbhNotificationsApi.Tests
{
    public class DynamoDbIntegrationTests<TStartup> where TStartup : class
    {
        private const string Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMTIyNDE2MjU4ODQ1MjgxMDQxNDAiLCJlbWFpbCI6ImRlbmlzZS5udWRnZUBoYWNrbmV5Lmdvdi51ayIsImlzcyI6IkhhY2tuZXkiLCJuYW1lIjoiRGVuaXNlIE51ZGdlIiwiZ3JvdXBzIjpbInNvbWUtdmFsaWQtZ29vZ2xlLWdyb3VwIl0sImlhdCI6MTYzOTQxNzE4OX0.Rai_olTwhVugBY8L8bpyhSGxX3lLB-ZLqxlSDQh96nE";
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
            EnsureEnvVarConfigured("REQUIRED_GOOGL_GROUPS", "some-valid-google-group");
            _factory = new DynamoDbMockWebApplicationFactory<TStartup>(_tables);
            Client = _factory.CreateClient();
            Client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(Token);
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
            //if (null != Client)
            //    Client.Dispose();
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
