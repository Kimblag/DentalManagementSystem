using DentalSystem.Domain.Entities;
using DentalSystem.Domain.ValueObjects;

namespace DentalSystem.Domain.Tests.Builder
{
    public class SpecialtyBuilder
    {

        public static Specialty CreateActiveWithOneTreatment(
            string? name = null,
            string? description = null)
        {
            var treatments = new List<Treatment>
            {
                TreatmentBuilder.CreateValid()
            };

            return new Specialty(
                NameBuilder.CreateSpecialtyName(name),
                treatments,
                DescriptionBuilder.CreateSpecialtyDescription(description)
            );
        }

        public static Specialty CreateActiveWithTreatments(
            IEnumerable<Treatment> treatments,
            string? specialtyName = null,
            string? specialtyDescription = null)
        {
            return new Specialty(
                NameBuilder.CreateSpecialtyName(specialtyName),
                [.. treatments],
                DescriptionBuilder.CreateSpecialtyDescription(specialtyDescription)
            );
        }

        public static Specialty CreateActiveWithMultipleTreatments()
        {
            var treatment1 = TreatmentBuilder.CreateValid();
            var treatment2 = TreatmentBuilder.CreateValid(
                "Clear Aligners",
                25.0m,
                "Removable transparent aligners for teeth correction.");

            var treatment3 = TreatmentBuilder.CreateValid(
                "Retainers",
                13.0m,
                "Devices used to maintain teeth position after treatment.");

            return CreateActiveWithTreatments(
                [treatment1, treatment2, treatment3]
            );
        }

        public static Specialty CreateInactive()
        {
            var specialty = CreateActiveWithOneTreatment();
            specialty.Deactivate();
            return specialty;
        }
    }
}
