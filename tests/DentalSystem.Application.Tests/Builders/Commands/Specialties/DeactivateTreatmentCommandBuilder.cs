using DentalSystem.Application.UseCases.Specialties.DeactivateTreatment;

namespace DentalSystem.Application.Tests.Builders.Commands.Specialties
{
    public class DeactivateTreatmentCommandBuilder
    {
        public static DeactivateTreatmentCommand Valid(Guid specialtyId, Guid treatmentId)
        {
            return new DeactivateTreatmentCommand
            {
               SpecialtyId = specialtyId,
               TreatmentId = treatmentId
            };
        }
    }
}
