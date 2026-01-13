using DentalSystem.Domain.ValueObjects;

namespace DentalSystem.Domain.Tests.Builder
{
    public static class NameBuilder
    {
        public const string DefaultSpecialtyName = "Orthodontics";
        public const string DefaultTreatmentName = "Braces";

        public static Name Create(string? value = null)
        {
            return new Name(value ?? DefaultTreatmentName);
        }

        public static Name CreateSpecialtyName(string? value = null)
        {
            return new Name(value ?? DefaultSpecialtyName);
        }
    }
}
