namespace DentalSystem.Application.UseCases.Specialties.AddTreatment
{
    public class TreatmentInput
    {
        public string Name { get; set; } = string.Empty;
        public decimal BaseCost { get; set; }
        public string? Description { get; set; } = string.Empty;
    }
}
