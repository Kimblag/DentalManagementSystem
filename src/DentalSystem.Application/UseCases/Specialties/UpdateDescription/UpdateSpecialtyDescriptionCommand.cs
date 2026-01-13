namespace DentalSystem.Application.UseCases.Specialties.UpdateDescription
{
    public class UpdateSpecialtyDescriptionCommand
    {
        public Guid SpecialtyId { get; set; }
        public string? Description { get; set; } = string.Empty; // if the user wants to reset it, must send an empty value
    }
}
