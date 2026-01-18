using DentalSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;

namespace DentalSystem.Infrastructure.Tests.Helpers
{
    public static class DbContextHelper
    {
        public static (DentalSystemDbContext cotext, SqliteConnection connection) CreateDbContext()
        {
            // Creates connection SQLite in memory
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            // configure db context options using connection
            var options = new DbContextOptionsBuilder<DentalSystemDbContext>()
                .UseSqlite(connection)
                .Options;

            // create actual db context 
            var context = new DentalSystemDbContext(options);


            // Ensure that database is created with mappings and constraints
            context.Database.EnsureCreated();


            return (context, connection);
        }
    }
}
