using System.ComponentModel.DataAnnotations;

namespace DentalSystem.Application.DTOs.Specialties.Inputs
{
    public record CreateSpecialtyRequest(
        [Required(ErrorMessage = "The name is required.")]
        [MaxLength(100, ErrorMessage = "The name cannot exceed 100 characters.")]
        string Name,

        [MaxLength(250, ErrorMessage = "The Description cannot exceed 250 characters.")]
        string? Description)
    {
    }
}
