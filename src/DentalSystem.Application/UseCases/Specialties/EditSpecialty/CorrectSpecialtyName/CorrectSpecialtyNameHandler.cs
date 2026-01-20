using DentalSystem.Application.Exceptions;
using DentalSystem.Application.Ports.Persistence;
using DentalSystem.Application.Ports.Repositories;

namespace DentalSystem.Application.UseCases.Specialties.EditSpecialty.CorrectSpecialtyName
{
    public sealed class CorrectSpecialtyNameHandler(ISpecialtyRepository repository, IUnitOfWork unitOfWork)
    {
        private readonly ISpecialtyRepository _repository = repository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Handle(CorrectSpecialtyNameCommand command, CancellationToken cancellationToken)
        {
            var specialty = await _repository.GetByIdAsync(command.SpecialtyId, cancellationToken)
                ?? throw new NotFoundException("Specialty",
                $"Specialty with id {command.SpecialtyId} was not found.");

            specialty.CorrectName(command.Name);

            await _unitOfWork.CommitAsync();
        }
    }
}
