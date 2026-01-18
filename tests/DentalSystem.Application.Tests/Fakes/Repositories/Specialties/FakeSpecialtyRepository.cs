using DentalSystem.Application.Ports.Repositories;
using DentalSystem.Domain.Entities;

namespace DentalSystem.Application.Tests.Fakes.Repositories.Specialties
{
    public sealed class FakeSpecialtyRepository : ISpecialtyRepository
    {
        private readonly Dictionary<Guid, Specialty> _storage = [];

        public Task<Specialty?> GetByIdAsync(
            Guid specialtyId,
            CancellationToken cancellationToken = default)
        {
            _storage.TryGetValue(specialtyId, out var specialty);
            return Task.FromResult(specialty);
        }

        public void Add(Specialty specialty)
        {
            _storage[specialty.SpecialtyId] = specialty;
        }
    }
}
