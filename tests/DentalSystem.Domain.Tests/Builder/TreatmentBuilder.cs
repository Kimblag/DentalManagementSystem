
using DentalSystem.Domain.Entities;

namespace DentalSystem.Domain.Tests.Builder
{
    public static class TreatmentBuilder
    {
        public const decimal defaultBaseCost = 10.0m;

        public static Treatment CreateValid(
            string? name = null,
            decimal? baseCost = null,
            string? description = null)
        {
            return new Treatment(
                NameBuilder.Create(name),
                baseCost ?? defaultBaseCost,
                DescriptionBuilder.CreateSpecialtyDescription(description)
            );
        }

        public static Treatment CreateInactive(string? name = null)
        {
            var treatment = CreateValid(name);
            treatment.Deactivate();
            return treatment;
        }
    }
}
