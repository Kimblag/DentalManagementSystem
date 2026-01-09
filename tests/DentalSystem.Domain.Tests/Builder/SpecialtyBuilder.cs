using DentalSystem.Domain.Entities;

namespace DentalSystem.Domain.Tests.Builder
{
    public static class SpecialtyBuilder
    {
        // default values
        public const string defaultName = "Orthodontics";
        public const string defaultDescription = "Focuses on correcting teeth and jaw alignment issues.";
    
        public static Specialty CreateActiveWithOneTreatment(string? name = null, string? description = null)
        {
            List<Treatment> treatments = new List<Treatment>
            {
                TreatmentBuilder.CreateValid()
            };

            return new Specialty(
               name ?? defaultName,
               treatments,
               description ?? defaultDescription
           );
        }

        public static Specialty CreateActiveWithTreatments(
           IEnumerable<Treatment> treatments,
           string? specialtyName = null,
           string? specialtyDescription = null)
        {
            return new Specialty(
                specialtyName ?? defaultName,
                [.. treatments],
                specialtyDescription ?? defaultDescription
            );
        }


        public static Specialty CreateActiveWithMultipleTreatments()
        {
            Treatment treatment1 = TreatmentBuilder.CreateValid();
            Treatment treatment2 = TreatmentBuilder.CreateValid("Clear Aligners", 25.0m, "Removable transparent aligners for teeth correction.");
            Treatment treatment3 = TreatmentBuilder.CreateValid("Retainers", 13.0m, "Devices used to maintain teeth position after treatment.");

            return CreateActiveWithTreatments([treatment1, treatment2, treatment3]);
        }
    }
}
