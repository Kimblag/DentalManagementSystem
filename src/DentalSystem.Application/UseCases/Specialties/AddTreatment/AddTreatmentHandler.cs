using DentalSystem.Application.Exceptions;
using DentalSystem.Application.Ports.Persistence;
using DentalSystem.Application.Ports.Repositories;

namespace DentalSystem.Application.UseCases.Specialties.AddTreatment
{
    public sealed class AddTreatmentHandler(ISpecialtyRepository repository,
        IUnitOfWork unitOfWork)
    {
        private readonly ISpecialtyRepository _repository = repository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Handle(AddTreatmentCommand command, CancellationToken cancellationToken)
        {
            var specialty = await _repository.GetByIdAsync(command.SpecialtyId, cancellationToken)
                ?? throw new SpecialtyNotFoundException();

            specialty.AddTreatment(
                command.Treatment.Name,
                command.Treatment.BaseCost,
                command.Treatment.Description
            );

            await _unitOfWork.CommitAsync();
        }
    }
}
