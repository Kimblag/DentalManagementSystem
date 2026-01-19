namespace DentalSystem.Application.UseCases.Specialties.ReactivateSpecialty
{
    public sealed class ReactivateSpecialtyCommand
    {
        public Guid SpecialtyId { get; }

        public ReactivateSpecialtyCommand(Guid specialtyId)
        {
            if (specialtyId == Guid.Empty)
                throw new ArgumentException("SpecialtyId cannot be empty.");

            SpecialtyId = specialtyId;
        }
    }
}
