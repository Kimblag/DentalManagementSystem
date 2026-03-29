using System.ComponentModel.DataAnnotations;

namespace DentalSystem.Application.DTOs.Specialties.Inputs
{
    public record ArchiveTreatmentRequest(
        [property: Required(ErrorMessage = "Specialty ID is required.")]
        Guid SpecialtyId,

        [property: Required(ErrorMessage = "The treatment code is required.")]
        [property: MaxLength(7, ErrorMessage = "The treatment code cannot exceed 7 characters.")]
        string TreatmentCode
        )
    {
    }
}
