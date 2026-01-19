using DentalSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;

namespace DentalSystem.Infrastructure.Tests.Helpers
{
    public static class DbContextHelper
    {
        public static DentalSystemDbContext CreateDbContext(
            SqliteConnection connection,
            bool ensureCreated = false)
        {
            // configure db context options using connection
            var options = new DbContextOptionsBuilder<DentalSystemDbContext>()
                .UseSqlite(connection)
                .Options;

            // create actual db context 
            var context = new DentalSystemDbContext(options);


            // Ensure that database is created with mappings and constraints
            if(ensureCreated)
                context.Database.EnsureCreated();


            return context;
        }

        public static SqliteConnection CreateInMemoryConnection()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            return connection;
        }
    }
}
