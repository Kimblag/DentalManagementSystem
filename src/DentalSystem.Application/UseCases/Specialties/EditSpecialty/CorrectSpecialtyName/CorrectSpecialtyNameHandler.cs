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
            var specialty = await _repository.GetById(command.SpecialtyId, cancellationToken)
                ?? throw new SpecialtyNotFoundException();

            specialty.CorrectName(command.Name);

            await _repository.Save(specialty, cancellationToken);

            if (_unitOfWork.HasChanges())
            {
                _unitOfWork.Commit();
            }
        }
    }
}
