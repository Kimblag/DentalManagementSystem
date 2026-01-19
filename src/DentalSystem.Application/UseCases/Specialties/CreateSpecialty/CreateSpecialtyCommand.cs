using DentalSystem.Application.Contracts.Specialties;

namespace DentalSystem.Application.UseCases.Specialties.CreateSpecialty
{
    public sealed class CreateSpecialtyCommand
    {
        public string Name { get; } = string.Empty;
        public string? Description { get; } = null;
        public List<TreatmentInput> Treatments { get; } = [];

        public CreateSpecialtyCommand(string name, List<TreatmentInput> treatments, string? description)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description;
            Treatments = treatments ?? throw new ArgumentNullException(nameof(treatments));
        }
    }
}
