using DentalSystem.Application.DTOs.Specialties.Inputs;
using DentalSystem.Application.DTOs.Specialties.Outputs;
using DentalSystem.Application.Exceptions;
using DentalSystem.Application.Interfaces;
using DentalSystem.Application.Interfaces.Repositories;
using DentalSystem.Application.Mappers.Specialties;
using DentalSystem.Domain.Aggregates.Specialty;
using DentalSystem.Domain.ValueObjects;

namespace DentalSystem.Application.UseCases.Specialties
{
    public sealed class CreateSpecialtyUseCase(ISpecialtyRepository repository, IUnitOfWork unitOfWork)
    {
        private readonly ISpecialtyRepository _repository = repository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<SpecialtyResponse> HandleAsync(CreateSpecialtyRequest request)
        {
            // VOs
            Name name = new(request.Name);

            // check if specialty already exists
            bool nameExists = await _repository.ExistsByNameAsync(name);
            if (nameExists) throw new ApplicationConflictException($"A specialty with the name {request.Name} already exists");

            // construir la specialty
            Specialty specialty = new(name, request.Description);

            // persistir
            await _repository.AddAsync(specialty);
            await _unitOfWork.SaveChangesAsync();

            return specialty.ToDto();
        }
    }
}
