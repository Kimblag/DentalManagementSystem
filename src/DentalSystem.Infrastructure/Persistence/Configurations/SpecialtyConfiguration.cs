using DentalSystem.Domain.Aggregates.Specialty;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace DentalSystem.Infrastructure.Persistence.Configurations
{
    public class SpecialtyConfiguration : IEntityTypeConfiguration<Specialty>
    {
        public void Configure(EntityTypeBuilder<Specialty> builder)
        {
            // definir cómo se llamará la tabla y cuál será la primary key
            builder.ToTable("Specialties");
            builder.HasKey(s => s.Id);

            // traducir un objeto complejo (VO Name)
            // ejecuto Property porque devuelve un objeto que puede permitir configurar propiedades
            builder.Property(s => s.Name)
                .HasConversion(
                nameVo => nameVo.Value, // cómo se guarda en la BD
                dbString => new Domain.ValueObjects.Name(dbString) // cómo leerlo desde la BD
                ).HasColumnName("Name") // el nombre de la columna
                .HasMaxLength(100)
                .IsRequired();

            // definir la columna name como única.
            builder.HasIndex(s => s.Name)
                .IsUnique();

            builder.Property(s => s.Description)
                .HasColumnName("Description")
                .HasMaxLength(250);

            // mapear el enum SpecialtyStatus
            builder.Property(s => s.Status)
                .HasConversion<string>() // indico que se debe convertir a un string como "ACTIVE"
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(s => s.CreatedAt)
                .HasColumnName("CreatedAt")
                .HasColumnType("datetime2")
                .IsRequired();

            // mapear la lista: Metadata y SetPropertyAccesMode le dice a EF
            // que le doy acceso VIP para que lea mi lista interna _treatments
            builder.Metadata
                .FindNavigation(nameof(Specialty.Treatments))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);


            /* Shadow properties */

            builder.Property<DateTime>("UpdatedAt")
                .HasColumnType("datetime2")
                .IsRequired();


            /* Relations */
            // has many with one indica que una especialidad tiene muchos tratamientos,
            // y un tratamiento pertenece solo a una. 
            builder.HasMany(s => s.Treatments)
                .WithOne() // no tiene parámetros porque Treatment no tiene la propiedad Specialty.
                .HasForeignKey("SpecialtyId")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
