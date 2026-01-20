using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DentalSystem.Infrastructure.Persistence
{
    public  class DentalSystemDbContextFactory : IDesignTimeDbContextFactory<DentalSystemDbContext>
    {
        public DentalSystemDbContext CreateDbContext(string[] args)
        {
            // Build configuration to get connection string
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<DentalSystemDbContext>();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            builder.UseSqlServer(connectionString);

            return new DentalSystemDbContext(builder.Options);
        }
    }
}
