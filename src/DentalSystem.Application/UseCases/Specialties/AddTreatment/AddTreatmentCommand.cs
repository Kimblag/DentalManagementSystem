namespace DentalSystem.Application.UseCases.Specialties.AddTreatment
{
    public class AddTreatmentCommand
    {
        public Guid SpecialtyId { get; set; }
        public TreatmentInput Treatment { get; set; } = null!;
    }
}
