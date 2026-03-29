using DentalSystem.Domain.Aggregates.Specialty;
using DentalSystem.Domain.ValueObjects;
using DentalSystem.Domain.ValueObjects.Specialty;

namespace DentalSystem.Application.Interfaces.Repositories
{
    public interface ISpecialtyRepository
    {
        Task<Specialty?> GetByIdAsync(Guid id);

        Task<bool> ExistsByNameAsync(Name name);

        Task<bool> ExistsTreatmentCodeAsync(TreatmentCode code);

        Task AddAsync(Specialty specialty);

        Task UpdateAsync(Specialty specialty);

    }
}
