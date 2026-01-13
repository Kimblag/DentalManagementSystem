namespace DentalSystem.Application.UseCases.Specialties.DeactivateTreatment
{
    public class DeactivateTreatmentCommand
    {
        public Guid SpecialtyId { get; set; }
        public Guid TreatmentId { get; set; }
    }
}
