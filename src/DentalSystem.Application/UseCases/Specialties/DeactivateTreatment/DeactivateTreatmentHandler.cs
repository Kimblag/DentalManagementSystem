using DentalSystem.Application.Exceptions;
using DentalSystem.Application.Ports.Persistence;
using DentalSystem.Application.Ports.Repositories;

namespace DentalSystem.Application.UseCases.Specialties.DeactivateTreatment
{
    public sealed class DeactivateTreatmentHandler(ISpecialtyRepository repository, IUnitOfWork unitOfWork)
    {
        private readonly ISpecialtyRepository _repository = repository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Handle(DeactivateTreatmentCommand command, CancellationToken cancellationToken)
        {
            // Search the specialty
            var specialty = await _repository.GetByIdAsync(command.SpecialtyId, cancellationToken)
               ?? throw new SpecialtyNotFoundException();

           // deactivate
            specialty.DeactivateTreatment(command.TreatmentId);

            // save the change
            _repository.Add(specialty);

            if (_unitOfWork.HasChanges())
            {
                await _unitOfWork.CommitAsync();
            }
        }
    }
}
