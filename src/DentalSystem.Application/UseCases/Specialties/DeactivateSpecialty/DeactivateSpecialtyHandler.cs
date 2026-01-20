using DentalSystem.Application.Exceptions;
using DentalSystem.Application.Exceptions.Specialties;
using DentalSystem.Application.Ports.Persistence;
using DentalSystem.Application.Ports.Repositories;

namespace DentalSystem.Application.UseCases.Specialties.DeactivateSpecialty
{
    public class DeactivateSpecialtyHandler(ISpecialtyRepository repository, IUnitOfWork unitOfWork)
    {
        private readonly ISpecialtyRepository _repository = repository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Handle(DeactivateSpecialtyCommand command, CancellationToken cancellationToken)
        {
            // Get the specialty
            var specialty = await _repository.GetByIdAsync(command.SpecialtyId, cancellationToken)
                ?? throw new NotFoundException("Specialty",
                $"Specialty with id {command.SpecialtyId} was not found.");

            // Ask to domain to deactivate the specialty
            specialty.Deactivate();
            
            await _unitOfWork.CommitAsync();
        }
    }
}
