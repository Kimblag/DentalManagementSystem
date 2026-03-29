using System.ComponentModel.DataAnnotations;

namespace DentalSystem.Application.DTOs.Specialty.Inputs
{
    public record ArchiveSpecialtyRequest(
        [property: Required(ErrorMessage = "Specialty ID is required.")]
        Guid SpecialtyId
        )
    {
    }
}
