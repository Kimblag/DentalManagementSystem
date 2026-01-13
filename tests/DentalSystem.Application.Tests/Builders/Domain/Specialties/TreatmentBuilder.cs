using DentalSystem.Domain.Entities;
using DentalSystem.Domain.ValueObjects;

namespace DentalSystem.Application.Tests.Builders.Domain.Specialties
{
    public static class TreatmentBuilder
    {
        public const string DefaultName = "Braces";
        public const decimal DefaultCost = 10m;
        public const string DefaultDescription = "A treatment";

        public static Treatment Active(
            string? name = null,
            decimal? cost = null,
            string? description = null)
        {
            return new Treatment(
                new Name(name ?? DefaultName),
                cost ?? DefaultCost,
                new Description(description ?? DefaultDescription)
            );
        }

        public static Treatment Inactive(string? name = null)
        {
            var treatment = Active(name);
            treatment.Deactivate();
            return treatment;
        }
    }
}
