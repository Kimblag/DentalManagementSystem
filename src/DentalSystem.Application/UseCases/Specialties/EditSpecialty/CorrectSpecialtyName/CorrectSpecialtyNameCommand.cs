
namespace DentalSystem.Application.UseCases.Specialties.EditSpecialty.CorrectSpecialtyName
{
    public sealed class CorrectSpecialtyNameCommand
    {
        public Guid SpecialtyId { get; }
        public string Name { get; }

        public CorrectSpecialtyNameCommand(Guid specialtyId, string name)
        {
            if (specialtyId == Guid.Empty)
                throw new ArgumentException("SpecialtyId cannot be empty.");

            Name = name ?? throw new ArgumentNullException(nameof(name));
            SpecialtyId = specialtyId;
        }
    }
}
