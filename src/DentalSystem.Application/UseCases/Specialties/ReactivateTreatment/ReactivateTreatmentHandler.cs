using DentalSystem.Application.Exceptions;
using DentalSystem.Application.Exceptions.Specialties;
using DentalSystem.Application.Ports.Persistence;
using DentalSystem.Application.Ports.Repositories;

namespace DentalSystem.Application.UseCases.Specialties.ReactivateTreatment
{
    public sealed class ReactivateTreatmentHandler(ISpecialtyRepository repository, IUnitOfWork unitOfWork)
    {
        private readonly ISpecialtyRepository _repository = repository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Handle(ReactivateTreatmentCommand command, CancellationToken cancellationToken)
        {
            // Search the specialty
            var specialty = await _repository.GetByIdAsync(command.SpecialtyId, cancellationToken)
               ?? throw new NotFoundException("Specialty",
                $"Specialty with id {command.SpecialtyId} was not found.");

            // reactivate
            specialty.ReactivateTreatment(command.TreatmentId);

            // save the change
            await _unitOfWork.CommitAsync();
        }
    }
}
