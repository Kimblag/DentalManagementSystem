using DentalSystem.Application.Exceptions;
using DentalSystem.Application.Ports.Repositories;
using DentalSystem.Domain.ValueObjects;

namespace DentalSystem.Application.UseCases.Specialties.UpdateTreatmentDetails
{
    public class UpdateTreatmentDetailsHandler(ISpecialtyRepository repository)
    {
        private readonly ISpecialtyRepository _repository = repository;

        public async Task Handle(UpdateTreatmentDetailsCommand command)
        {
            // search the aggregate
            var specialty = await _repository.GetById(command.SpecialtyId) 
                ?? throw new SpecialtyNotFoundException();

            Description? treatmentDescription = command.TreatmentDescription is null
                ? null
                : new Description(command.TreatmentDescription);

            specialty.UpdateTreatmentDetails(command.TreatmentId, 
                command.TreatmentBaseCost,
                treatmentDescription, 
                command.TreatmentName
                );

            await _repository.Save(specialty);
        }
    }
}
