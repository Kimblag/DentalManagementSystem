using DentalSystem.Application.Exceptions;
using DentalSystem.Application.Ports.Persistence;
using DentalSystem.Application.Ports.Repositories;

namespace DentalSystem.Application.UseCases.Specialties.ReactivateSpecialty
{
    public class ReactivateSpecialtyHandler(ISpecialtyRepository repository, IUnitOfWork unitOfWork)
    {
        private readonly ISpecialtyRepository _repository = repository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Handle(Guid specialtyId, CancellationToken cancellationToken)
        {
            // Get the specialty
            var specialty = await _repository.GetByIdAsync(specialtyId, cancellationToken)
                ?? throw new SpecialtyNotFoundException();

            // Ask to domain to reactivate the specialty
            specialty.Reactivate();

            // Save the new state
            _repository.Add(specialty);

            if (_unitOfWork.HasChanges())
            {
                await _unitOfWork.CommitAsync();
            }
        }
    }
}
