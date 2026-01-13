namespace DentalSystem.Application.UseCases.Specialties.UpdateTreatmentDetails
{
    public class UpdateTreatmentDetailsCommand
    {
        public Guid SpecialtyId { get; set; }
        public Guid TreatmentId { get; set; }
        public string? TreatmentName { get; set; } = null;
        public decimal? TreatmentBaseCost { get; set; } = null;
        public string? TreatmentDescription { get; set; } = null;
    }
}
