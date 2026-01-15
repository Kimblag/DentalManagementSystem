
namespace DentalSystem.Application.UseCases.Specialties.EditTreatment.CorrectTreatmentName
{
    public sealed class CorrectTreatmentNameCommand
    {
        public Guid SpecialtyId { get;  }
        public Guid TreatmentId { get; }
        public string Name { get; } = string.Empty;


        public CorrectTreatmentNameCommand(Guid specialtyId, Guid treatmentId, string name)
        {
            if (specialtyId == Guid.Empty || treatmentId == Guid.Empty || string.IsNullOrEmpty(name))
                throw new ArgumentException("SpecialtyID, TreatmentId and Name cannot be empty");

            SpecialtyId = specialtyId;
            TreatmentId = treatmentId;
            Name = name;
        }
    }
}
