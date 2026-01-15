using DentalSystem.Application.Tests.Builders.Domain.Specialties;
using DentalSystem.Application.Tests.Fakes.Persistence;
using DentalSystem.Application.Tests.Fakes.Repositories.Specialties;
using DentalSystem.Application.UseCases.Specialties.ReactivateSpecialty;
using DentalSystem.Domain.Entities;
using DentalSystem.Domain.Exceptions.Specialties;

namespace DentalSystem.Application.Tests.UseCases.Specialties.ReactivateSpecialtyTests
{
    public sealed class ReactivateSpecialtyTest
    {
        // Happy path
        [Fact]
        public async Task Handle_WhenSpecialtyIsInactive_ShouldActivateAndPersistChanges()
        {
            // Arrange
            Specialty specialty = SpecialtyBuilder.CreateActiveWithOneTreatment();
            specialty.Deactivate();

            // Add to repo
            FakeUnitOfWork unitOfWork = new();
            FakeSpecialtyRepository repository = new(unitOfWork);
            
            repository.Add(specialty);

            var handler = new ReactivateSpecialtyHandler(repository, unitOfWork);

            // Act
            await handler.Handle(specialty.SpecialtyId, CancellationToken.None);

            // Assert
            var stored = await repository.GetById(specialty.SpecialtyId, CancellationToken.None);

            // Check if specialty is inactive
            Assert.True(stored!.Status.IsActive);
            // All treatments should be inactive
            Assert.All(
                   stored.Treatments,
                   t => Assert.True(t.Status.IsActive)
               );
            // It must be persisted
            Assert.True(unitOfWork.WasCommitted);
        }

        // Errors
        [Fact]
        public async Task Handle_WhenSpecialtyIsAlreadyActive_ShouldThrowInvalidStatusTransitionException_AndNotPersist()
        {
            // Arrange
            Specialty specialty = SpecialtyBuilder.CreateActiveWithOneTreatment();

            // Add to repo
            FakeUnitOfWork unitOfWork = new();
            FakeSpecialtyRepository repository = new(unitOfWork);

            repository.Add(specialty);
            var handler = new ReactivateSpecialtyHandler(repository, unitOfWork);

            // Act
            // Assert
            await Assert.ThrowsAnyAsync<InvalidStatusTransitionException>(async () =>
            {
                await handler.Handle(specialty.SpecialtyId, CancellationToken.None);
            });
            Assert.False(unitOfWork.WasCommitted);
        }
    }
}
