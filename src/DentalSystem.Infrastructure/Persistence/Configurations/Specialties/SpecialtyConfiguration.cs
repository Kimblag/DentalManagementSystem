using DentalSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DentalSystem.Infrastructure.Persistence.Configurations.Specialties
{
    internal class SpecialtyConfiguration : IEntityTypeConfiguration<Specialty>
    {
        public void Configure(EntityTypeBuilder<Specialty> builder)
        {
            // Methods: https://www.entityframeworktutorial.net/efcore/fluent-api-in-entity-framework-core.aspx

            // Configures the property or list of properties as Primary Key.
            builder.HasKey(s => s.SpecialtyId);
            builder.Property(s => s.SpecialtyId).ValueGeneratedNever();

            // Map ValueObjects
            builder.OwnsOne(s => s.Name, name =>
            {
                name.Property(n => n.Value)
                .HasColumnName("Name")
                .HasMaxLength(100)
                .IsRequired();
            });

            builder.HasIndex(s => s.Name.Value)
                .IsUnique();

            builder.OwnsOne(s => s.Description, description =>
            {
                description.Property(d => d.Value)
                .HasColumnName("Description")
                .HasMaxLength(500)
                .IsRequired(false);
            });

            builder.OwnsOne(s => s.Status, status =>
            {
                status.Property(s => s.IsActive)
                .HasColumnName("IsActive")
                .IsRequired();
            });

            // Relationships 1 specialty has many treatments 1...n
            builder.HasMany(typeof(Treatment), "_treatments")
                .WithOne() // each treatment is asociated to only one specialty, I do not expose this relation because it breaks the aggregate
                .HasForeignKey("SpecialtyId")
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable(tb =>
            {
                tb.HasCheckConstraint(
                    "CK_Specialty_Description_Length",
                    "[Description] IS NULL OR LEN([Description]) >= 3"
                );
                tb.HasCheckConstraint(
                   "CK_Specialty_Name_Length",
                   "LEN([Name]) >= 3"
               );
            });


        }
    }
}
