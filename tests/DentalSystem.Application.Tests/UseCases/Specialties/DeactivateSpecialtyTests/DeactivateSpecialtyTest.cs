using DentalSystem.Application.Exceptions;
using DentalSystem.Application.Tests.Builders.Domain.Specialties;
using DentalSystem.Application.Tests.Fakes.Persistence;
using DentalSystem.Application.Tests.Fakes.Repositories.Specialties;
using DentalSystem.Application.UseCases.Specialties.DeactivateSpecialty;
using DentalSystem.Domain.Entities;
using DentalSystem.Domain.Exceptions.Specialties;

namespace DentalSystem.Application.Tests.UseCases.Specialties.DeactivateSpecialtyTests
{
    public sealed class DeactivateSpecialtyTest
    {

        // Happy path
        [Fact]
        public async Task Handle_WhenSpecialtyIsActive_ShouldDeactivateAndPersistChanges()
        {
            // Arrange
            Specialty specialty = SpecialtyBuilder.CreateActiveWithOneTreatment();

            // Add to repo
            FakeUnitOfWork unitOfWork = new();
            FakeSpecialtyRepository repository = new();
            repository.Add(specialty);

            DeactivateSpecialtyHandler handler = new(repository, unitOfWork);

            DeactivateSpecialtyCommand command = new(specialty.SpecialtyId);
            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            // It must be persisted

            var stored = await repository.GetByIdAsync(specialty.SpecialtyId, CancellationToken.None);
            // Check observable changes
            Assert.True(stored!.Status.IsInactive);
            Assert.All(
                   stored.Treatments,
                   t => Assert.True(t.Status.IsInactive)
               );
        }


        // Specialty not found
        [Fact]
        public async Task Handle_WhenSpecialtyDoesNotExist_ShouldThrowSpecialtyNotFoundException()
        {
            // Arrange
            FakeUnitOfWork unitOfWork = new();
            FakeSpecialtyRepository repository = new();

            DeactivateSpecialtyHandler handler = new(repository, unitOfWork);
            DeactivateSpecialtyCommand command = new(Guid.NewGuid());
            // Act
            // Assert
            await Assert.ThrowsAsync<SpecialtyNotFoundException>(async () =>
            {
                await handler.Handle(command, CancellationToken.None);
            });
        }


        // Specialty already inactive
        [Fact]
        public async Task Handle_WhenSpecialtyIsAlreadyInactive_ShouldThrowInvalidStatusTransitionException_AndNotPersist()
        {
            // Arrange
            Specialty specialty = SpecialtyBuilder.CreateInactive();

            FakeUnitOfWork unitOfWork = new();
            FakeSpecialtyRepository repository = new();

            repository.Add(specialty);

            DeactivateSpecialtyHandler handler = new(repository, unitOfWork);
            DeactivateSpecialtyCommand command = new(specialty.SpecialtyId);
            // Act
            // Assert
            await Assert.ThrowsAsync<InvalidStatusTransitionException>(async () =>
            {
                await handler.Handle(command, CancellationToken.None);
            });
        }
    }
}
