using DentalSystem.Application.UseCases.Specialties.Create;

namespace DentalSystem.Application.Tests.Builders.Commands.Specialties
{
    public static class TreatmentInputBuilder
    {
        public static TreatmentInput Valid() => new()
        {
            Name = "Braces",
            BaseCost = 10m,
            Description = "A treatment"
        };

        public static TreatmentInput WithName(string name)
        {
            var input = Valid();
            input.Name = name;
            return input;
        }
    }
}
