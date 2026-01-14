
using DentalSystem.Application.Exceptions;
using DentalSystem.Application.Tests.Builders.Domain.Specialties;
using DentalSystem.Application.Tests.Fakes.Repositories.Specialties;
using DentalSystem.Application.UseCases.Specialties.Deactivate;
using DentalSystem.Domain.Entities;
using DentalSystem.Domain.Enums;
using DentalSystem.Domain.Exceptions;

namespace DentalSystem.Application.Tests.UseCases.Specialties.Deactivate
{
    public class DeactivateSpecialtyTests
    {

        // Happy path
        [Fact]
        public async Task Handle_WhenSpecialtyIsActive_ShouldDeactivateAndPersistChanges()
        {
            // Arrange
            Specialty specialty = SpecialtyBuilder.ActiveWithOneTreatment();

            // Add to repo
            var repository = new FakeSpecialtyRepository();
            repository.Add(specialty);
            var handler = new DeactivateSpecialtyHandler(repository);

            // Act
            await handler.Handle(specialty.SpecialtyId);

            // Assert
            var storedInFakeSpecialty = await repository.GetById(specialty.SpecialtyId);
            Assert.NotNull(storedInFakeSpecialty);
            // Check if specialty is inactive
            Assert.Equal(EntityStatus.Inactive, storedInFakeSpecialty.Status);
            // All treatments should be inactive
            Assert.All(
                   storedInFakeSpecialty.Treatments,
                   t => Assert.True(t.Status.IsInactive)
               );
            // IT must be persisted
            Assert.True(repository.SaveWasCalled);
        }


        // Specialty not found
        [Fact]
        public async Task Handle_WhenSpecialtyDoesNotExist_ShouldThrowSpecialtyNotFoundException()
        {
            // Arrange
            var repository = new FakeSpecialtyRepository();
            var handler = new DeactivateSpecialtyHandler(repository);

            // Act
            // Assert
            await Assert.ThrowsAnyAsync<SpecialtyNotFoundException>(async () =>
            {
                await handler.Handle(Guid.NewGuid());
            });
            Assert.False(repository.SaveWasCalled);
        }


        // Specialty already inactive
        [Fact]
        public async Task Handle_WhenSpecialtyIsAlreadyInactive_ShouldThrowInvalidStatusTransitionException_AndNotPersist()
        {
            // Arrange
            Specialty specialty = SpecialtyBuilder.ActiveWithOneTreatment();
            specialty.Deactivate();
            
            var repository = new FakeSpecialtyRepository();
            repository.Add(specialty);
            var handler = new DeactivateSpecialtyHandler(repository);

            // Act
            // Assert
            await Assert.ThrowsAnyAsync<DomainException>(async () =>
            {
                await handler.Handle(specialty.SpecialtyId);
            });
            Assert.False(repository.SaveWasCalled);
        }
    }
}
