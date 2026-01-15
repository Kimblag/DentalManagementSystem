namespace DentalSystem.Application.UseCases.Specialties.EditSpecialty.UpdateSpecialtyDescription
{
    public sealed class UpdateSpecialtyDescriptionCommand
    {
        public Guid SpecialtyId { get; }
        public string? Description { get; } = null; // if the user wants to reset it, must send an empty value

        public UpdateSpecialtyDescriptionCommand(Guid specialtyId, string? description)
        {
            if (specialtyId == Guid.Empty)
                throw new ArgumentException("SpecialtyId cannot be empty.");

            Description = description;
            SpecialtyId = specialtyId;
        }
    }
}
