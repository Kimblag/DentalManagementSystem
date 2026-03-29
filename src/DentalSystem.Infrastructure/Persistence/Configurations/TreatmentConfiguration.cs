using DentalSystem.Domain.Aggregates.Specialty;
using DentalSystem.Domain.ValueObjects.Specialty;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DentalSystem.Infrastructure.Persistence.Configurations
{
    public class TreatmentConfiguration : IEntityTypeConfiguration<Treatment>
    {
        public void Configure(EntityTypeBuilder<Treatment> builder)
        {
            builder.ToTable("Treatments");

            // Code es un VO, indico a EF que es la primary key
            builder.HasKey(t => t.Code);

            builder.Property(t => t.Code)
                .HasConversion(
                    code => code.Value, // para guardar en la bd (string)
                    dbValue => new TreatmentCode(dbValue)) // cargarlo como VO al leer
                .HasColumnName("Code")
                .HasMaxLength(7)
                .IsRequired();

            builder.Property(s => s.Name)
                .HasConversion(
                nameVo => nameVo.Value, // cómo se guarda en la BD
                dbString => new Domain.ValueObjects.Name(dbString) // cómo leerlo desde la BD
                ).HasColumnName("Name") // el nombre de la columna
                .HasMaxLength(100)
                .IsRequired();


            builder.Property(s => s.Description)
                .HasColumnName("Description")
                .HasMaxLength(250);


            builder.Property(s => s.Status)
              .HasConversion<string>() // indico que se debe convertir a un string como "ACTIVE"
              .HasMaxLength(20)
              .IsRequired();

            // Money es un VO con 2 propiedades, para poder mapearlo NET >=8 permite
            // usar ComplexProperty para que le diga a EF que Modey es un simple contenedor
            // y NO otra tabla
            builder.ComplexProperty(t => t.BaseCost, moneyBuilder =>
            {
                // configurar las properties de Money
                moneyBuilder.Property(m => m.Amount)
                .HasColumnName("BaseCost_Amount")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

                moneyBuilder.Property(m => m.Currency)
                .HasColumnName("BaseCost_Currency")
                .HasMaxLength(3)
                .IsRequired();
            });

            /* Shadow properties */
            builder.Property<DateTime>("CreatedAt")
                .HasColumnType("datetime2")
                .IsRequired();

            builder.Property<DateTime>("UpdatedAt")
                .HasColumnType("datetime2")
                .IsRequired(false);
        }
    }
}
