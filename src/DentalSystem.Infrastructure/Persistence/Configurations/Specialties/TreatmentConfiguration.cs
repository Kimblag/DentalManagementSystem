using DentalSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DentalSystem.Infrastructure.Persistence.Configurations.Specialties
{
    public sealed class TreatmentConfiguration : IEntityTypeConfiguration<Treatment>
    {
        public void Configure(EntityTypeBuilder<Treatment> builder)
        {
            builder.HasKey(t => t.TreatmentId);
            builder.Property(t => t.TreatmentId).ValueGeneratedNever();

            builder.Property<Guid>("SpecialtyId");
            builder.HasIndex("SpecialtyId");

            // Map Values
            builder.OwnsOne(s => s.Name, name =>
            {
                name.Property(n => n.Value)
                .HasColumnName("Name")
                .HasMaxLength(100)
                .IsRequired();
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

            // Map property
            builder.Property(t => t.BaseCost)
                .HasPrecision(10, 2)
                .IsRequired();

        }
    }
}
