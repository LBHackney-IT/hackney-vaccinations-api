

using System.Linq;
using FluentAssertions;
using LbhNotificationsApi.Tests.V1.Helper;
using Microsoft.EntityFrameworkCore.Storage;
using Xunit;

namespace LbhNotificationsApi.Tests.V1.Infrastructure
{
    //TODO: Remove this file if Postgres is not being used
    public class DatabaseContextTest : DatabaseTests
    {
        [Fact]
        public void CanGetADatabaseEntity()
        {
            var databaseEntity = DatabaseEntityHelper.CreateDatabaseEntity();

            DatabaseContext.Add(databaseEntity);
            DatabaseContext.SaveChanges();

            var result = DatabaseContext.DatabaseEntities.ToList().FirstOrDefault();

          result.Should().BeEquivalentTo(databaseEntity);
        }

        public DatabaseContextTest(IDbContextTransaction transaction) : base(transaction)
        {
        }
    }
}
