using System.ComponentModel.DataAnnotations;

namespace DentalSystem.Application.DTOs.Specialties.Inputs
{
    public record RenameSpecialtyRequest(
        [property: Required(ErrorMessage = "Specialty ID is required.")]
        Guid SpecialtyId,


        [property: Required(ErrorMessage = "The name is required.")]
        [property: MaxLength(100, ErrorMessage = "The name cannot exceed 100 characters.")]
        string Name
        )
    {
    }
}
