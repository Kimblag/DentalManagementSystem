using DentalSystem.Application.Ports.Repositories;
using DentalSystem.Domain.Entities;
using DentalSystem.Domain.ValueObjects;

namespace DentalSystem.Application.UseCases.Specialties.Create
{
    public class CreateSpecialtyHandler(ISpecialtyRepository repository)
    {
        private readonly ISpecialtyRepository _repository = repository;

        public async Task Handle(CreateSpecialtyCommand command)
        {
            List<Treatment> treatments = [];

            foreach (TreatmentInput treatment in command.Treatments)
            {
                Description? treatmentDescription = treatment.Description is null
                   ? null
                   : new Description(treatment.Description);
                treatments.Add(new Treatment(new Name(treatment.Name), treatment.BaseCost, treatmentDescription));
            }

            Description? description = command.Description is null
                ? null
                : new Description(command.Description);

            Specialty specialty = new(new Name(command.Name), treatments, description);
            await _repository.Save(specialty);
        }
    }
}
