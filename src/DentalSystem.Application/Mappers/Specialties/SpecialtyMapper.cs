using DentalSystem.Application.DTOs.Specialties.Outputs;
using DentalSystem.Domain.Aggregates.Specialty;

namespace DentalSystem.Application.Mappers.Specialties
{
    public static class SpecialtyMapper
    {
        public static SpecialtyResponse ToDto(this Specialty specialty) // this extiende la clase specialty
        {
            return new SpecialtyResponse(
                specialty.Id,
                specialty.Name.Value,
                specialty.Description,
                specialty.Status.ToString(),
                [.. specialty.Treatments.Select(t => t.ToDto())],
                specialty.CreatedAt
                );
        }
    }
}
