using DentalSystem.Application.UseCases.Specialties.Create;
using DentalSystem.Domain.Entities;

namespace DentalSystem.Application.Tests.Builders.Commands.Specialties
{
    public static class CreateSpecialtyCommandBuilder
    {

        public static CreateSpecialtyCommand Valid()
        {
            return new CreateSpecialtyCommand
            {
                Name = "Orthodontics",
                Description = "Specialty description",
                Treatments =
                [
                    new() { Name = "Braces", BaseCost = 10, Description = "A treatment" }
                ]
            };
        }

        public static CreateSpecialtyCommand InvalidWithTreatments(params TreatmentInput[] treatments)
        {
            return new CreateSpecialtyCommand
            {
                Name = "Orthodontics",
                Description = "Specialty description",
                Treatments = treatments.ToList()
            };
        }
    }
}
