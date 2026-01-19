using DentalSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DentalSystem.Infrastructure.Persistence.Configurations.Specialties
{
    public sealed class SpecialtyConfiguration : IEntityTypeConfiguration<Specialty>
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

                // because Name is a Owned type, we use the name of the column directly
                name.HasIndex(n => n.Value)
                .IsUnique();
            });

            

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


            // First, let me point out that this navigation uses this
            // backing field and not the property, otherwise it gives
            // an error because it tries to map the other Treatments property 
            builder.Metadata
                .FindNavigation(nameof(Specialty.Treatments))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);

            // Relationships 1 specialty has many treatments 1...n
            builder.HasMany(s => s.Treatments)
                .WithOne() // each treatment is asociated to only one specialty, I do not expose this relation because it breaks the aggregate
                .HasForeignKey("SpecialtyId")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
