using DentalSystem.Domain.ValueObjects;

namespace DentalSystem.Domain.Tests.Builder
{
    public static class NameBuilder
    {
        public const string DefaultSpecialtyName = "Orthodontics";

        public static Name CreateSpecialtyName(string? value = null)
        {
            return new Name(value ?? DefaultSpecialtyName);
        }
    }
}
