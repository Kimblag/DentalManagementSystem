using DentalSystem.Domain.Aggregates.Specialty;
using Microsoft.EntityFrameworkCore;

namespace DentalSystem.Infrastructure.Persistence
{
    public class DentalSystemDbContext : DbContext
    {
        // no setear Treatment porque es un agregado
        public DbSet<Specialty> Specialties { get; set; }

        // Options: indicar  al motor a qué servidor específico debe conectarse al arrancar la aplicación.
        public DentalSystemDbContext(DbContextOptions<DentalSystemDbContext> options)
            : base(options)
        {

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // buscar en todo el proyecto cualquier clase que tenga 
            // configuraciones de Fluent API y las aplica
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DentalSystemDbContext).Assembly);
        }
    }
}
