namespace DentalSystem.Application.DTOs.Specialties.Outputs
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
