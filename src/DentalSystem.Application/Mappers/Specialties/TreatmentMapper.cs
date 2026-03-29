using DentalSystem.Application.DTOs.Specialties.Outputs;
using DentalSystem.Domain.Aggregates.Specialty;

namespace DentalSystem.Application.Mappers.Specialties
{
    public static class TreatmentMapper
    {
        public static TreatmentResponse ToDto(this Treatment treatment) // this extiende Treatments
        {
            return new TreatmentResponse(
                treatment.Code.Value,
                treatment.Name.Value,
                treatment.BaseCost.Amount,
                treatment.BaseCost.Currency,
                treatment.Status.ToString(),
                treatment.Description
                );
        }
    }
}
