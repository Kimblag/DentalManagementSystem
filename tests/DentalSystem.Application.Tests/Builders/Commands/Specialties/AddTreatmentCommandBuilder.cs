using DentalSystem.Application.UseCases.Specialties.AddTreatment;

namespace DentalSystem.Application.Tests.Builders.Commands.Specialties
{
    public static class AddTreatmentCommandBuilder
    {
        public static AddTreatmentCommand Valid(Guid specialtyId)
        {
            return new AddTreatmentCommand
            {
                SpecialtyId = specialtyId,
                Treatment = new TreatmentInput
                {
                    Name = "Aligners",
                    BaseCost = 20,
                    Description = "Second treatment"
                }
            };
        }

        public static AddTreatmentCommand WithName(Guid specialtyId, string name)
        {
            var command = Valid(specialtyId);
            command.Treatment.Name = name;
            return command;
        }
    }
}
