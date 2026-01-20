using DentalSystem.Application.Exceptions;
using DentalSystem.Application.Exceptions.Specialties;
using DentalSystem.Application.Ports.Persistence;
using DentalSystem.Application.Ports.Repositories;

namespace DentalSystem.Application.UseCases.Specialties.EditSpecialty.UpdateSpecialtyDescription
{
    public sealed class UpdateSpecialtyDescriptionHandler(ISpecialtyRepository repository, IUnitOfWork unitOfWork)
    {
        private readonly ISpecialtyRepository _repository = repository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork; 

        public async Task Handle(UpdateSpecialtyDescriptionCommand command, CancellationToken cancellationToken)
        {
            var specialty = await _repository.GetByIdAsync(command.SpecialtyId, cancellationToken)
                ?? throw new NotFoundException("Specialty",
                $"Specialty with id {command.SpecialtyId} was not found.");

            specialty.UpdateDescription(command.Description);

            await _unitOfWork.CommitAsync();
        }

    }
}
