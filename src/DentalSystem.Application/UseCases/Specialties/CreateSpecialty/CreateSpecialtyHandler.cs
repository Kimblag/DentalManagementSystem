using DentalSystem.Application.Ports.Persistence;
using DentalSystem.Application.Ports.Repositories;
using DentalSystem.Domain.Entities;

namespace DentalSystem.Application.UseCases.Specialties.CreateSpecialty
{
    public sealed class CreateSpecialtyHandler(ISpecialtyRepository repository,
        IUnitOfWork unitOfWork)
    {
        private readonly ISpecialtyRepository _repository = repository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Handle(CreateSpecialtyCommand command, CancellationToken cancellationToken)
        {
            var treatmentsTuples = command.Treatments
                .Select(t => (t.Name, t.BaseCost, t.Description));

            Specialty specialty = new(command.Name, treatmentsTuples, command.Description);
            _repository.Add(specialty);

            // successfull creation
            await _unitOfWork.CommitAsync();
        }
    }
}
