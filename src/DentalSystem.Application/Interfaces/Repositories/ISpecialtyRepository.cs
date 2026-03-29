using DentalSystem.Domain.Aggregates.Specialty;

namespace DentalSystem.Application.Interfaces.Repositories
{
    public interface ISpecialtyRepository
    {
        Task<ISpecialtyRepository?> GetByIdAsync(Guid id);

        Task<bool> ExistsByNameAsync(string name);

        Task<bool> ExistsTreatmentCodeAsync(string code);

        Task AddAsync(Specialty specialty);

        Task UpdateAsync(Specialty specialty);

    }
}
