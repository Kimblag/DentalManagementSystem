using DentalSystem.Application.Exceptions;
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
            var specialty = await _repository.GetById(command.SpecialtyId, cancellationToken)
               ?? throw new SpecialtyNotFoundException();

            specialty.ChangeTreatmentBaseCost(command.TreatmentId, command.BaseCost);

            // save the change
            await _repository.Save(specialty, cancellationToken);

            if (_unitOfWork.HasChanges())
            {
                _unitOfWork.Commit();
            }
        }
    }
}