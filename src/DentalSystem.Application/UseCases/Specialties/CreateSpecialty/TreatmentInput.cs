namespace DentalSystem.Application.UseCases.Specialties.CreateSpecialty
{
    public sealed class TreatmentInput
    {
        public string Name { get; }
        public decimal BaseCost { get; }
        public string? Description { get; }

        public TreatmentInput(string name, decimal baseCost, string? description)
        {
            Name = name;
            BaseCost = baseCost;
            Description = description;
        }
    }
}
