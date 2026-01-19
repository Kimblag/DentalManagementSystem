namespace DentalSystem.Application.UseCases.Specialties.DeactivateSpecialty
{
    public sealed class DeactivateSpecialtyCommand
    {
        public Guid SpecialtyId { get; }

        public DeactivateSpecialtyCommand(Guid specialtyId)
        {
            if (specialtyId == Guid.Empty)
                throw new ArgumentException("SpecialtyId cannot be empty.");

            SpecialtyId = specialtyId;
        }
    }
}
