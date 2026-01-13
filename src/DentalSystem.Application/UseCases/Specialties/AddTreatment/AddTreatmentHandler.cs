using DentalSystem.Application.Exceptions;
using DentalSystem.Application.Ports.Repositories;
using DentalSystem.Domain.Entities;
using DentalSystem.Domain.ValueObjects;

namespace DentalSystem.Application.UseCases.Specialties.AddTreatment
{
    public class AddTreatmentHandler(ISpecialtyRepository repository)
    {
        private readonly ISpecialtyRepository _repository = repository;

        public async Task Handle(AddTreatmentCommand command)
        {
            // Search the specialty
            var specialty = await _repository.GetById(command.SpecialtyId)
               ?? throw new SpecialtyNotFoundException();

            Treatment newTreatment = new(
                new Name(command.Treatment.Name), 
                command.Treatment.BaseCost, 
                command.Treatment.Description);

            // add the new treatment
            specialty.AddTreatment(newTreatment);

            // save the change
            await _repository.Save(specialty);
        }
    }
}
