using DentalSystem.Application.Ports.Repositories;
using DentalSystem.Application.Ports.Persistence;
using DentalSystem.Domain.Entities;

namespace DentalSystem.Application.Tests.Fakes.Repositories.Specialties
{
    public sealed class FakeSpecialtyRepository : ISpecialtyRepository
    {
        private readonly Dictionary<Guid, Specialty> _storage = [];
        private readonly IUnitOfWork _unitOfWork;

        public FakeSpecialtyRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<Specialty?> GetById(Guid specialtyId, CancellationToken cancellationToken)
        {
            if (_storage.TryGetValue(specialtyId, out var specialty))
            {
                _unitOfWork.Track(specialty);
                return Task.FromResult<Specialty?>(specialty);
            }

            return Task.FromResult<Specialty?>(null);
        }

        public Task Save(Specialty specialty, CancellationToken cancellationToken)
        {
            _storage[specialty.SpecialtyId] = specialty;
            return Task.CompletedTask;
        }

        public void Add(Specialty specialty)
        {
            _storage[specialty.SpecialtyId] = specialty;
        }
    }
}
