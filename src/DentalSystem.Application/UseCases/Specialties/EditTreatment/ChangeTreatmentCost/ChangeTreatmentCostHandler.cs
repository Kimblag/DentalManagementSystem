using DentalSystem.Application.Exceptions;
using DentalSystem.Application.Exceptions.Specialties;
using DentalSystem.Application.Ports.Persistence;
using DentalSystem.Application.Ports.Repositories;

namespace DentalSystem.Application.UseCases.Specialties.EditTreatment.ChangeTreatmentCost
{
    public sealed class ChangeTreatmentCostHandler(ISpecialtyRepository repository, IUnitOfWork unitOfWork)
    {
        private readonly ISpecialtyRepository _repository = repository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Handle(ChangeTreatmentCostCommand command, CancellationToken cancellationToken)
        {
            // Search the specialty
            var specialty = await _repository.GetByIdAsync(command.SpecialtyId, cancellationToken)
               ?? throw new NotFoundException("Specialty",
                $"Specialty with id {command.SpecialtyId} was not found.");

            specialty.ChangeTreatmentBaseCost(command.TreatmentId, command.BaseCost);

            // save the change
            await _unitOfWork.CommitAsync();
        }
    }
}