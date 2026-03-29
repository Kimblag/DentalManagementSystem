using System.ComponentModel.DataAnnotations;

namespace DentalSystem.Application.DTOs.Specialty.Inputs
{
    public record UpdateTreatmentDescRequest(
        [property: Required(ErrorMessage = "Specialty ID is required.")]
        Guid SpecialtyId,

        [property: Required(ErrorMessage = "The treatment code is required.")]
        [property: MaxLength(7, ErrorMessage = "The treatment code cannot exceed 7 characters.")]
        string TreatmentCode,

        [property: MaxLength(250, ErrorMessage = "The description cannot exceed 250 characters.")]
        string? Description
        )
    {
    }
}
