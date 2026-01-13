namespace DentalSystem.Application.UseCases.Specialties.CorrectName
{
    public class CorrectSpecialtyNameCommand
    {
        public Guid SpecialtyId { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
