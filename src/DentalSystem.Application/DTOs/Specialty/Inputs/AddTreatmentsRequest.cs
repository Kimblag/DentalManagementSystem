using System.ComponentModel.DataAnnotations;

namespace DentalSystem.Application.DTOs.Specialty.Inputs
{
    public record AddTreatmentsRequest(
        [property: Required(ErrorMessage = "Specialty ID is required.")]
        Guid SpecialtyId,

        [property: Required(ErrorMessage = "Treatments are required.")]
        [property: MinLength(1, ErrorMessage = "At least one treatment is required.")]
        List<TreatmentItemDto> Treatments
        )
    { }
}
