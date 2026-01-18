using Microsoft.EntityFrameworkCore;

namespace DentalSystem.Infrastructure.Persistence
{
    // DbContext is a technical unit of work that:
    // Maintains an in-memory map of entities ↔ rows
    // Knows how to translate objects to SQL
    // Knows what has changed and what hasn't
    // Executes transactions

    // 1. inherites from DbContext
    public class DentalSystemDbContext : DbContext
    {
        // 2. constructor using DbContextOptions -> is the configuration already built. (inmutable)
        // DO NOT confuse with DbContextOptionsBuilder -> tool for build the configuration. (mutable)
        public DentalSystemDbContext(DbContextOptions<DentalSystemDbContext> options)
            : base(options)
        {
            
        }

        //It's the place where EF learns what my model is like. OnModelCreating = object mapping → database
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Search this assembly for all classes that implement IEntityTypeConfiguration<T> and apply them automatically
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DentalSystemDbContext).Assembly);
        }
    }
}
