using LbhNotificationsApi.V1.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace LbhNotificationsApi.Tests
{
    
    public abstract class DatabaseTests
    {
        private readonly IDbContextTransaction _transaction;
        protected DatabaseContext DatabaseContext { get; set; }

        protected DatabaseTests(IDbContextTransaction transaction)
        {
            var builder = new DbContextOptionsBuilder();
            builder.UseNpgsql(ConnectionString.TestDatabase());
            DatabaseContext = new DatabaseContext(builder.Options);

            DatabaseContext.Database.EnsureCreated();
            _transaction = transaction;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private bool _disposed;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing || _disposed) return;
            _transaction.Rollback();
            _transaction.Dispose();
            _disposed = true;
        }
        
    }
}
