using DentalSystem.Application.Ports.Repositories;
using DentalSystem.Domain.Entities;

namespace DentalSystem.Application.Tests.Fakes.Repositories.Specialties
{
    public class FakeSpecialtyRepository : ISpecialtyRepository
    {
        private readonly Dictionary<Guid, Specialty> _storage = [];
        public bool SaveWasCalled { get; private set; } = false;

        public void Add(Specialty specialty)
        {
            _storage[specialty.SpecialtyId] = specialty;
        }

        public Task<Specialty?> GetById(Guid specialtyId)
        {
            _storage.TryGetValue(specialtyId, out var specialty);
            return Task.FromResult(specialty);
        }

        public Task Save(Specialty specialty)
        {
            _storage[specialty.SpecialtyId] = specialty;
            SaveWasCalled = true;
            return Task.CompletedTask;
        }
    }
}
