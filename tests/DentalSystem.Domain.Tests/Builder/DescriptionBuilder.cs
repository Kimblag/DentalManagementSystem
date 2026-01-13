using DentalSystem.Domain.ValueObjects;

namespace DentalSystem.Domain.Tests.Builder
{
    public static class DescriptionBuilder
    {
        public const string DefaultSpecialtyDescription = "Description of a specialty";
        public const string DefaultTreatmentDescription = "Description of a treatment";

        public static Description Create(string? value = null)
        {
            return new Description(value ?? DefaultTreatmentDescription);
        }

        public static Description CreateSpecialtyDescription(string? value = null)
        {
            return new Description(value ?? DefaultSpecialtyDescription);
        }
    }
}
