using System.ComponentModel.DataAnnotations;

namespace DentalSystem.Application.DTOs.Specialties.Inputs
{
    public record ArchiveSpecialtyRequest(
        [property: Required(ErrorMessage = "Specialty ID is required.")]
        Guid SpecialtyId
        )
    {
    }
}
