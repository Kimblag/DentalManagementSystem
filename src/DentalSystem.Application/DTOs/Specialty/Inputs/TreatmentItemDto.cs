using System.ComponentModel.DataAnnotations;

namespace DentalSystem.Application.DTOs.Specialty.Inputs
{
    public record TreatmentItemDto(
        [property: Required, MaxLength(7)]
        string Code,

        [property: Required(ErrorMessage = "The name is required.")]
        [property: MaxLength(100, ErrorMessage = "The name cannot exceed 100 characters."),
         MinLength(3, ErrorMessage = "The Name must contains at least 3 characters.")]
        string Name,

        [property: Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
        decimal Amount,

        [property: Required, StringLength(3, MinimumLength = 3)]
        string Currency,

        [property: MaxLength(250, ErrorMessage = "The Description cannot exceed 250 characters.")]
        string? Description
        )
    {
    }
}
