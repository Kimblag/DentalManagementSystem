
using DentalSystem.Domain.Entities;

namespace DentalSystem.Domain.Tests.Builder
{
    public static class TreatmentBuilder
    {

        public const string defaultName = "Braces";
        public const string defaultDescription = "Metal or ceramic devices to straighten teeth.";
        public const decimal defaultBaseCost = 10.0m;


        public static Treatment CreateValid(string? name = null, decimal? baseCost = null, string? description = null)
        {
            return new Treatment(
                name ?? defaultName, 
                baseCost ?? defaultBaseCost, 
                description ?? defaultDescription);
        }
    }
}
