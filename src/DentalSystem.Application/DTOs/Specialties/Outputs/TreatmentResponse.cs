namespace DentalSystem.Application.DTOs.Specialties.Outputs
{
    public record TreatmentResponse(
        string Code,
        string Name,
        decimal BaseCost_Amount,
        string BaseCost_Currency,
        string Status,
        string? Description
        )
    {
    }
}
