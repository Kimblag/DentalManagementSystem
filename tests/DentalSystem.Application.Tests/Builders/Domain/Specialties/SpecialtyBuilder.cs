using DentalSystem.Domain.Entities;

namespace DentalSystem.Application.Tests.Builders.Domain.Specialties
{
    public static class SpecialtyBuilder
    {
        private const string DefaultSpecialtyName = "Orthodontics";
        private const string DefaultSpecialtyDescription = "Description of a specialty";

        private const string DefaultTreatmentName = "Braces";
        private const decimal DefaultTreatmentCost = 10.0m;
        private const string DefaultTreatmentDescription = "Description of a treatment";

        public static Specialty CreateActiveWithOneTreatment(
            string? specialtyName = null,
            string? specialtyDescription = null)
        {
            IEnumerable<(string name, decimal baseCost, string? description)> treatments =
            [
                (DefaultTreatmentName, DefaultTreatmentCost, DefaultTreatmentDescription)
            ];

            return new Specialty(
                specialtyName ?? DefaultSpecialtyName,
                treatments,
                specialtyDescription ?? DefaultSpecialtyDescription
            );
        }

        public static Specialty CreateActiveWithTreatments(
            IEnumerable<(string name, decimal baseCost, string? description)> treatments,
            string? specialtyName = null,
            string? specialtyDescription = null)
        {
            return new Specialty(
                specialtyName ?? DefaultSpecialtyName,
                treatments,
                specialtyDescription ?? DefaultSpecialtyDescription
            );
        }

        public static Specialty CreateActiveWithTwoDistinctTreatments()
        {
            IEnumerable<(string name, decimal baseCost, string? description)> treatments =
            [
                ("Clear Aligners", 25.0m, "Removable transparent aligners."),
                ("Retainers", 13.0m, "Devices used to maintain teeth position.")
            ];

            return CreateActiveWithTreatments(treatments);
        }

        public static Specialty CreateActiveWithTwoDistinctTreatmentsAndOneInactiveTreatment()
        {
            IEnumerable<(string name, decimal baseCost, string? description)> treatments =
            [
                ("Clear Aligners", 25.0m, "Removable transparent aligners."),
                ("Retainers", 13.0m, "Devices used to maintain teeth position.")
            ];

            var specialty = CreateActiveWithTreatments(treatments);

            var treatmentToDeactivate = specialty.Treatments.First();
            specialty.DeactivateTreatment(treatmentToDeactivate.TreatmentId);

            return specialty;
        }

        public static Specialty CreateInactive(string? specialtyName = null)
        {
            var specialty = CreateActiveWithOneTreatment(specialtyName);
            specialty.Deactivate();
            return specialty;
        }
    }
}
