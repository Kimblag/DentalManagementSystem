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
                treatments.Add(new Treatment(new Name(treatment.Name), treatment.BaseCost, treatment.Description));
            }
            Specialty specialty = new(new Name(command.Name), treatments, command.Description);
            await _repository.Save(specialty);
        }
    }
}
