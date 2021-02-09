using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;

namespace Service.Test.Repository
{
    public class ConnectionFactory : IDisposable
    {

        private bool disposedValue = false; // To detect redundant calls

        public TestContext CreateContextForInMemory()
        {
            var option = new DbContextOptionsBuilder<TestContext>()
                .UseInMemoryDatabase(databaseName: "Test_Database")
                .Options;

            var context = new TestContext(option);
            if (context != null)
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }

            return context;
        }

        public TestContext CreateContextForSQLite()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var option = new DbContextOptionsBuilder<TestContext>().UseSqlite(connection).Options;

            var context = new TestContext(option);

            if (context != null)
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }

            return context;
        }


        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
