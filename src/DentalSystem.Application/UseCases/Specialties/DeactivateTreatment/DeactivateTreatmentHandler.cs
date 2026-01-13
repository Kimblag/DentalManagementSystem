using DentalSystem.Application.Exceptions;
using DentalSystem.Application.Ports.Repositories;

namespace DentalSystem.Application.UseCases.Specialties.DeactivateTreatment
{
    public class DeactivateTreatmentHandler(ISpecialtyRepository repository)
    {
        private readonly ISpecialtyRepository _repository = repository;

        public async Task Handle(DeactivateTreatmentCommand command)
        {
            // Search the specialty
            var specialty = await _repository.GetById(command.SpecialtyId)
               ?? throw new SpecialtyNotFoundException();

           // deactivate
            specialty.DeactivateTreatment(command.TreatmentId);

            // save the change
            await _repository.Save(specialty);
        }
    }
}
