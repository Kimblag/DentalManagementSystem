namespace DentalSystem.Application.UseCases.Specialties.Create
{
    public class CreateSpecialtyCommand
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public List<TreatmentInput> Treatments { get; set; } = [];
    }
}
