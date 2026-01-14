using DentalSystem.Application.Tests.Builders.Domain.Specialties;
using DentalSystem.Application.Tests.Fakes.Repositories.Specialties;
using DentalSystem.Application.UseCases.Specialties.Reactivate;
using DentalSystem.Domain.Entities;
using DentalSystem.Domain.Enums;
using DentalSystem.Domain.Exceptions;
using DentalSystem.Domain.Exceptions.Specialties;

namespace DentalSystem.Application.Tests.UseCases.Specialties.Reactivate
{
    public class ReactivateSpecialtyTest
    {
        // Happy path
        [Fact]
        public async Task Handle_WhenSpecialtyIsInactive_ShouldActivateAndPersistChanges()
        {
            // Arrange
            Specialty specialty = SpecialtyBuilder.ActiveWithOneTreatment();
            specialty.Deactivate();

            // Add to repo
            FakeSpecialtyRepository repository = new();
            repository.Add(specialty);
            var handler = new ReactivateSpecialtyHandler(repository);

            // Act
            await handler.Handle(specialty.SpecialtyId);

            // Assert
            var storedInFakeSpecialty = await repository.GetById(specialty.SpecialtyId);
            Assert.NotNull(storedInFakeSpecialty);
            // Check if specialty is inactive
            Assert.Equal(EntityStatus.Active, storedInFakeSpecialty.Status);
            // All treatments should be inactive
            Assert.All(
                   storedInFakeSpecialty.Treatments,
                   t => Assert.True(t.Status.IsActive)
               );
            // IT must be persisted
            Assert.True(repository.SaveWasCalled);
        }

        // Errors
        [Fact]
        public async Task Handle_WhenSpecialtyIsAlreadyActive_ShouldThrowInvalidStatusTransitionException_AndNotPersist()
        {
            // Arrange
            Specialty specialty = SpecialtyBuilder.ActiveWithOneTreatment();

            // Add to repo
            FakeSpecialtyRepository repository = new();
            repository.Add(specialty);
            var handler = new ReactivateSpecialtyHandler(repository);

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
