namespace DentalSystem.Api.Contracts.Specialties
{
    public sealed class AddTreatmentRequest
    {
        public string Name { get; init; } = string.Empty;
        public decimal BaseCost { get; init; }
        public string? Description { get; init; }
    }
}
