using System.ComponentModel.DataAnnotations;

namespace DentalSystem.Application.DTOs.Specialties.Inputs
{
    public record UpdateSpecialtyDescRequest(
        [property: Required(ErrorMessage = "Specialty ID is required.")]
        Guid SpecialtyId,


        [property: MaxLength(250, ErrorMessage = "The description cannot exceed 250 characters.")]
        string? Description
        )
    {
    }
}
