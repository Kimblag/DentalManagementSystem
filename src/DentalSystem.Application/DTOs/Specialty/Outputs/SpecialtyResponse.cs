namespace DentalSystem.Application.DTOs.Specialty.Outputs
{
    public record SpecialtyResponse(
        Guid Id,
        string Name,
        string? Description,
        string Status,
        IReadOnlyCollection<TreatmentResponse> Treatments
        )
    {
    }
}
