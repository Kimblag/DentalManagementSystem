using System.ComponentModel.DataAnnotations;

namespace DentalSystem.Application.DTOs.Specialties.Inputs
{
    public record CreateSpecialtyRequest(
        [property: Required(ErrorMessage = "The name is required.")]
        [property: MaxLength(100, ErrorMessage = "The name cannot exceed 100 characters.")]
        string Name,

        [property: MaxLength(250, ErrorMessage = "The Description cannot exceed 250 characters.")]
        string? Description)
    {
    }
}
