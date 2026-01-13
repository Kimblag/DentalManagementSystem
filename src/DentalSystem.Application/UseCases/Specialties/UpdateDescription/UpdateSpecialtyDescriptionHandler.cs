using DentalSystem.Application.Exceptions;
using DentalSystem.Application.Ports.Repositories;
using DentalSystem.Domain.ValueObjects;

namespace DentalSystem.Application.UseCases.Specialties.UpdateDescription
{
    public class UpdateSpecialtyDescriptionHandler(ISpecialtyRepository repository)
    {
        private readonly ISpecialtyRepository _repository = repository;

        public async Task Handle(UpdateSpecialtyDescriptionCommand command) 
        {
            // Get the specialty
            var specialty = await _repository.GetById(command.SpecialtyId)
                ?? throw new SpecialtyNotFoundException();

            // Ask to domain to change the description
            Description? description = command.Description is null
                ? null
                : new Description(command.Description);

            specialty.UpdateDescription(description);

            // Save it
            await _repository.Save(specialty);

        } 
    }
}
