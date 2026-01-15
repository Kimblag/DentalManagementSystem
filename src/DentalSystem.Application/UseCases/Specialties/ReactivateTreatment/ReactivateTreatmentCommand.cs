namespace DentalSystem.Application.UseCases.Specialties.ReactivateTreatment
{
    public sealed class ReactivateTreatmentCommand
    {
        public Guid SpecialtyId { get; }
        public Guid TreatmentId { get; }

        public ReactivateTreatmentCommand(Guid specialtyId, Guid treatmentId)
        {
            if (specialtyId == Guid.Empty || treatmentId == Guid.Empty)
                throw new ArgumentException("SpecialtyId and TreatmentId cannot be empty.");

            SpecialtyId = specialtyId;
            TreatmentId = treatmentId;
        }
    }
}
