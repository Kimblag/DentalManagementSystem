using DentalSystem.Application.Contracts.Specialties;

namespace DentalSystem.Application.UseCases.Specialties.AddTreatment
{
    public sealed class AddTreatmentCommand
    {
        public Guid SpecialtyId { get; }
        public TreatmentInput Treatment { get; }

        public AddTreatmentCommand(Guid specialtyId, TreatmentInput treatment)
        {
            if (specialtyId == Guid.Empty)
                throw new ArgumentException("SpecialtyId cannot be empty.");

            Treatment = treatment ?? throw new ArgumentNullException(nameof(treatment));
            SpecialtyId = specialtyId;
        }
    }
}
