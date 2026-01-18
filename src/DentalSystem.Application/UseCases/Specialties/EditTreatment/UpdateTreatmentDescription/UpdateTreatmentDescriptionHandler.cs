using DentalSystem.Application.Exceptions;
using DentalSystem.Application.Ports.Persistence;
using DentalSystem.Application.Ports.Repositories;

namespace DentalSystem.Application.UseCases.Specialties.EditTreatment.UpdateTreatmentDescription
{
    public sealed class UpdateTreatmentDescriptionHandler(ISpecialtyRepository repository, IUnitOfWork unitOfWork)
    {
        private readonly ISpecialtyRepository _repository = repository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Handle(UpdateTreatmentDescriptionCommand command, CancellationToken cancellationToken)
        {
            var specialty = await _repository.GetByIdAsync(command.SpecialtyId, cancellationToken) ??
                throw new SpecialtyNotFoundException();

            specialty.UpdateTreatmentDescription(command.TreatmentId, command.Description);

            await _unitOfWork.CommitAsync();
        }
    }
}
