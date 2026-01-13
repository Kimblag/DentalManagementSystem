using DentalSystem.Domain.Entities;
using DentalSystem.Domain.ValueObjects;

namespace DentalSystem.Application.Tests.Builders.Domain.Specialties
{
    public static class SpecialtyBuilder
    {
        public const string DefaultName = "Orthodontics";
        public const string DefaultDescription = "Specialty description";

        public static Specialty ActiveWithOneTreatment()
        {
            return new Specialty(
                new Name(DefaultName),
                [TreatmentBuilder.Active()],
                new Description(DefaultDescription)
            );
        }

        public static Specialty ActiveWithOneTreatmentAndName(string name)
        {
            return new Specialty(
                new Name(name),
                [TreatmentBuilder.Active()],
                new Description(DefaultDescription)
            );
        }

        public static Specialty ActiveWithTreatments(params Treatment[] treatments)
        {
            return new Specialty(
                new Name(DefaultName),
                [.. treatments],
                new Description(DefaultDescription)
            );
        }

        public static Specialty Inactive()
        {
            var specialty = ActiveWithOneTreatment();
            specialty.Deactivate();
            return specialty;
        }
    }
}
