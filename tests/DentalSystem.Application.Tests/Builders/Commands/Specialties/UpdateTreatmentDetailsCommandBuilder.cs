using DentalSystem.Application.UseCases.Specialties.UpdateTreatmentDetails;

namespace DentalSystem.Application.Tests.Builders.Commands.Specialties
{
    public static class UpdateTreatmentDetailsCommandBuilder
    {
        public static UpdateTreatmentDetailsCommand Valid(Guid specialtyId, Guid treatmentId, string? name = "Braces")
        {
            return new UpdateTreatmentDetailsCommand
            {
                SpecialtyId = specialtyId,
                TreatmentId = treatmentId,
                TreatmentName = name,
                TreatmentDescription = "Updated description",
                TreatmentBaseCost = 15
            };
        }

        public static UpdateTreatmentDetailsCommand Invalid(Guid specialtyId, Guid treatmentId)
        {
            return new UpdateTreatmentDetailsCommand
            {
                SpecialtyId = specialtyId,
                TreatmentId = treatmentId,
                TreatmentName = "!!",
                TreatmentDescription = "!!",
                TreatmentBaseCost = -100
            };
        }
    }
}
