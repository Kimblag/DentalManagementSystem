using System.ComponentModel.DataAnnotations;

namespace DentalSystem.Application.DTOs.Specialty.Inputs
{
    public record ActivateSpecialtyRequest(
        [property: Required(ErrorMessage = "Specialty ID is required.")]
        Guid SpecialtyId
        )
    {
    }
}
