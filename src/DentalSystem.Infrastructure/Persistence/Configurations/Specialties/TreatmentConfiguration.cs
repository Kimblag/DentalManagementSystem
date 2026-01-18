using DentalSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DentalSystem.Infrastructure.Persistence.Configurations.Specialties
{
    internal class TreatmentConfiguration : IEntityTypeConfiguration<Treatment>
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

            // Composite index.
            // The treatment name is unique within a specialty.
            builder.HasIndex(t => new { 
                SpecialtyId = EF.Property<Guid>(t, "SpecialtyId"), 
                Name = t.Name.Value 
            }).IsUnique();


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

            builder.ToTable(tb =>
            {
                tb.HasCheckConstraint(
                    "CK_Treatment_Description_Length",
                    "[Description] IS NULL OR LEN([Description]) >= 3"
                );

                tb.HasCheckConstraint(
                    "CK_Treatment_Name_Length",
                    "LEN([Name]) >= 3"
                );

                tb.HasCheckConstraint(
                    "CK_Treatment_BaseCost_Positive",
                    "[BaseCost] >= 0");
            });
        }
    }
}
