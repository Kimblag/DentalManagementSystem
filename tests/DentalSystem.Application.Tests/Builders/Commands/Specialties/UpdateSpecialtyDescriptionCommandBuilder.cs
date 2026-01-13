using DentalSystem.Application.UseCases.Specialties.UpdateDescription;

namespace DentalSystem.Application.Tests.Builders.Commands.Specialties
{
    public static class UpdateSpecialtyDescriptionCommandBuilder
    {
        public static UpdateSpecialtyDescriptionCommand Valid(Guid specialtyId, string description)
        {
            return new UpdateSpecialtyDescriptionCommand
            {
                SpecialtyId = specialtyId,
                Description = description,
            };
        }
    }
}
