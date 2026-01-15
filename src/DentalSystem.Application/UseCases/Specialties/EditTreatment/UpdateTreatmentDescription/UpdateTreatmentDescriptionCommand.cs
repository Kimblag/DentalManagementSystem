namespace DentalSystem.Application.UseCases.Specialties.EditTreatment.UpdateTreatmentDescription
{
    public sealed class UpdateTreatmentDescriptionCommand
    {
        public Guid SpecialtyId { get; }
        public Guid TreatmentId { get; }
        public string? Description { get; } = null;

        public UpdateTreatmentDescriptionCommand(Guid specialtyId, Guid treatmentId, string? description)
        {
            if (specialtyId == Guid.Empty || treatmentId == Guid.Empty)
                throw new ArgumentException("SpecialtyId and TreatmentId cannot be empty.");

            SpecialtyId = specialtyId;
            TreatmentId = treatmentId;
            Description = description;
        }
    }
}
