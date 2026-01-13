using DentalSystem.Application.Exceptions;
using DentalSystem.Application.Ports.Repositories;

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

            specialty.UpdateTreatmentDetails(command.TreatmentId, 
                command.TreatmentBaseCost, 
                command.TreatmentDescription, 
                command.TreatmentName
                );

            await _repository.Save(specialty);
        }
    }
}
