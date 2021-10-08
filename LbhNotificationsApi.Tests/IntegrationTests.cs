using System;
using System.Net.Http;
using LbhNotificationsApi.V1.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Npgsql;
namespace LbhNotificationsApi.Tests
{
    public class IntegrationTests<TStartup> where TStartup : class
    {
        private HttpClient Client { get; set; }
        private DatabaseContext DatabaseContext { get; set; }

        private readonly MockWebApplicationFactory<TStartup> _factory;
        private readonly IDbContextTransaction _transaction;


        protected IntegrationTests()
        {
            var connection = new NpgsqlConnection(ConnectionString.TestDatabase());
            connection.Open();
            var npgsqlCommand = connection.CreateCommand();
            npgsqlCommand.CommandText = "SET deadlock_timeout TO 30";
            npgsqlCommand.ExecuteNonQuery();

            var builder = new DbContextOptionsBuilder();
            builder.UseNpgsql(connection);
            EnsureEnvVarConfigured("DynamoDb_LocalMode", "true");
            EnsureEnvVarConfigured("DynamoDb_LocalServiceUrl", "http://localhost:8000");
            //_factory = new DynamoDbMockWebApplicationFactory<TStartup>(_tables);
            //Client = _factory.CreateClient();
            _factory = new MockWebApplicationFactory<TStartup>(connection);
            Client = _factory.CreateClient();
            DatabaseContext = new DatabaseContext(builder.Options);
            DatabaseContext.Database.EnsureCreated();
            _transaction = DatabaseContext.Database.BeginTransaction();
            //CleanupActions = new List<Action>();
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
            Client.Dispose();
            _transaction.Rollback();
            _transaction.Dispose();
        }
        private static void EnsureEnvVarConfigured(string name, string defaultValue)
        {
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable(name)))
                Environment.SetEnvironmentVariable(name, defaultValue);
        }
    }
}
