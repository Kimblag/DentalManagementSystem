using DentalSystem.Application.Interfaces;
using DentalSystem.Domain.Aggregates.Specialty;
using Microsoft.EntityFrameworkCore;

namespace DentalSystem.Infrastructure.Persistence
{
    public class DentalSystemDbContext : DbContext, IUnitOfWork
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

        /* automatizar el llenado de las columnas de auditoría (CreatedAt y UpdatedAt) */
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Se itera sobre todas las entidades que están siendo rastreadas y que han sido modificadas o agregadas
            foreach (var entry in ChangeTracker.Entries())
            {
                // Si la entidad es nueva (INSERT)
                if (entry.State == EntityState.Added)
                {
                    // Se verifica si la entidad tiene configurada la Shadow Property "CreatedAt"
                    if (entry.Metadata.FindProperty("CreatedAt") != null)
                    {
                        entry.Property("CreatedAt").CurrentValue = DateTime.UtcNow;
                    }
                }

                // Si la entidad fue modificada (UPDATE)
                if (entry.State == EntityState.Modified)
                {
                    // Se verifica si la entidad tiene configurada la Shadow Property "UpdatedAt"
                    if (entry.Metadata.FindProperty("UpdatedAt") != null)
                    {
                        entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
                    }
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
