using DentalSystem.Application.Exceptions;
using DentalSystem.Application.Ports.Repositories;

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
            specialty.UpdateDescription(command.Description);

            // Save it
            await _repository.Save(specialty);

        } 
    }
}
