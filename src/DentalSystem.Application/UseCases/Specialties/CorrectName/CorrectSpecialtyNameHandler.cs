using DentalSystem.Application.Exceptions;
using DentalSystem.Application.Ports.Repositories;

namespace DentalSystem.Application.UseCases.Specialties.CorrectName
{
    public class CorrectSpecialtyNameHandler(ISpecialtyRepository repository)
    {
        private readonly ISpecialtyRepository _repository = repository;

        public async Task Handle(CorrectSpecialtyNameCommand command)
        {
            var specialty = await _repository.GetById(command.SpecialtyId)
                ?? throw new SpecialtyNotFoundException();

            //Ask to domain to change the name
            specialty.CorrectName(command.Name);

            await _repository.Save(specialty);
        }
    }
}
