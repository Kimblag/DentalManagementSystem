using DentalSystem.Application.DTOs.Specialties.Outputs;
using DentalSystem.Application.Exceptions;
using DentalSystem.Application.Interfaces;
using DentalSystem.Application.Interfaces.Repositories;
using DentalSystem.Application.Mappers.Specialties;
using DentalSystem.Domain.Aggregates.Specialty;

namespace DentalSystem.Application.UseCases.Specialties
{
    public sealed class GetSpecialtyByIdUseCase(ISpecialtyRepository repository, IUnitOfWork unitOfWork)
    {
        private readonly ISpecialtyRepository _repository = repository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<SpecialtyResponse> HandleAsync(Guid id)
        {
            Specialty? specialty = await _repository.GetByIdAsync(id)
                ?? throw new ApplicationNotFoundException($"Specialty with id {id} not found.");
            return specialty.ToDto();
        }
    }
}
