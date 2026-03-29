using System.ComponentModel.DataAnnotations;

namespace DentalSystem.Application.DTOs.Specialty.Inputs
{
    public record UpdateTreatmentCostRequest(
        [property: Required(ErrorMessage = "Specialty ID is required.")]
        Guid SpecialtyId,

        [property: Required(ErrorMessage = "The treatment code is required.")]
        [property: MaxLength(7, ErrorMessage = "The treatment code cannot exceed 7 characters.")]
        string TreatmentCode,

        [property: Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
        decimal Amount,

        [property: Required, StringLength(3, MinimumLength = 3)]
        string Currency
        )
    {
    }
}
