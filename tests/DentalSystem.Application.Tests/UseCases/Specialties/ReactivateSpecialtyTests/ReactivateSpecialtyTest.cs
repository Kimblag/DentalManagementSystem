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
            FakeSpecialtyRepository repository = new();
            
            repository.Add(specialty);

            ReactivateSpecialtyHandler handler = new(repository, unitOfWork);
            ReactivateSpecialtyCommand command = new(specialty.SpecialtyId);
            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            var stored = await repository.GetByIdAsync(specialty.SpecialtyId, CancellationToken.None);

            // Check if specialty is inactive
            Assert.True(stored!.Status.IsActive);
            // All treatments should be inactive
            Assert.All(
                   stored.Treatments,
                   t => Assert.True(t.Status.IsActive)
               );
        }

        // Errors
        [Fact]
        public async Task Handle_WhenSpecialtyIsAlreadyActive_ShouldThrowInvalidStatusTransitionException_AndNotPersist()
        {
            // Arrange
            Specialty specialty = SpecialtyBuilder.CreateActiveWithOneTreatment();

            // Add to repo
            FakeUnitOfWork unitOfWork = new();
            FakeSpecialtyRepository repository = new();

            repository.Add(specialty);
            var handler = new ReactivateSpecialtyHandler(repository, unitOfWork);
            ReactivateSpecialtyCommand command = new(specialty.SpecialtyId);

            // Act
            // Assert
            await Assert.ThrowsAnyAsync<InvalidStatusTransitionException>(async () =>
            {
                await handler.Handle(command, CancellationToken.None);
            });
        }
    }
}
