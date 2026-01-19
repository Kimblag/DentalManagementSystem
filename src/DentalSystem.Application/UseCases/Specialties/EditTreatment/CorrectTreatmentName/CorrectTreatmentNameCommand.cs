
namespace DentalSystem.Application.UseCases.Specialties.EditTreatment.CorrectTreatmentName
{
    public sealed class CorrectTreatmentNameCommand
    {
        public Guid SpecialtyId { get;  }
        public Guid TreatmentId { get; }
        public string Name { get; } = string.Empty;


        public CorrectTreatmentNameCommand(Guid specialtyId, Guid treatmentId, string name)
        {
            if (specialtyId == Guid.Empty || treatmentId == Guid.Empty)
                throw new ArgumentException("SpecialtyID, TreatmentId cannot be empty");

            Name = name ?? throw new ArgumentNullException(nameof(name));
            SpecialtyId = specialtyId;
            TreatmentId = treatmentId;
        }
    }
}
