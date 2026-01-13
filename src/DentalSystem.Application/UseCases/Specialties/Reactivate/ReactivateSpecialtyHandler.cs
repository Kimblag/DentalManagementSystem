using DentalSystem.Application.Exceptions;
using DentalSystem.Application.Ports.Repositories;

namespace DentalSystem.Application.UseCases.Specialties.Reactivate
{
    public class ReactivateSpecialtyHandler(ISpecialtyRepository repository)
    {
        private readonly ISpecialtyRepository _repository = repository;

        public async Task Handle(Guid specialtyId)
        {
            // Get the specialty
            var specialty = await _repository.GetById(specialtyId)
                ?? throw new SpecialtyNotFoundException();

            // Ask to domain to reactivate the specialty
            specialty.Reactivate();

            // Save the new state
            await _repository.Save(specialty);
        }
    }
}
