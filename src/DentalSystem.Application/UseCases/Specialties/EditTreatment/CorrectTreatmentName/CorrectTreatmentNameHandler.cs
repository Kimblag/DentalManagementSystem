using DentalSystem.Application.Exceptions;
using DentalSystem.Application.Ports.Persistence;
using DentalSystem.Application.Ports.Repositories;

namespace DentalSystem.Application.UseCases.Specialties.EditTreatment.CorrectTreatmentName
{
    public sealed class CorrectTreatmentNameHandler(ISpecialtyRepository repository, IUnitOfWork unitOfWork)
    {
        private readonly ISpecialtyRepository _repository = repository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Handle(CorrectTreatmentNameCommand command, CancellationToken cancellationToken)
        {
            var specialty = await _repository.GetById(command.SpecialtyId, cancellationToken) ??
                throw new SpecialtyNotFoundException();

            specialty.CorrectTreatmentName(command.TreatmentId, command.Name);

            await _repository.Save(specialty, cancellationToken);

            if (_unitOfWork.HasChanges())
            {
                _unitOfWork.Commit();
            }
        }
    }
}
