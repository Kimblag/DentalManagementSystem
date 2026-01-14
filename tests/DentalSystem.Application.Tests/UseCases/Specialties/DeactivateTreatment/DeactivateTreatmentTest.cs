using DentalSystem.Application.Tests.Builders.Commands.Specialties;
using DentalSystem.Application.Tests.Builders.Domain.Specialties;
using DentalSystem.Application.Tests.Fakes.Repositories.Specialties;
using DentalSystem.Application.UseCases.Specialties.DeactivateTreatment;
using DentalSystem.Domain.Entities;
using DentalSystem.Domain.Enums;
using DentalSystem.Domain.Exceptions;
using DentalSystem.Domain.Exceptions.Specialties;

namespace DentalSystem.Application.Tests.UseCases.Specialties.DeactivateTreatment
{
    public class DeactivateTreatmentTest
    {
        // Happy path
        [Fact]
        public async Task Handle_WhenDeactivateAnExistingTreatment_ShouldDeactivateInSpecialtyAndPersistChanges()
        {
            // Arrange
            var aligners = TreatmentBuilder.Active(name: "Aligners", cost: 20, description: "Other treament");
            var braces = TreatmentBuilder.Active();
            Specialty specialty = SpecialtyBuilder.ActiveWithTreatments(braces, aligners);

            var repository = new FakeSpecialtyRepository();
            repository.Add(specialty);
            var handler = new DeactivateTreatmentHandler(repository);

            var command = DeactivateTreatmentCommandBuilder.Valid(specialty.SpecialtyId, aligners.TreatmentId);

            // Act
            await handler.Handle(command);

            // Assert
            var stored = await repository.GetById(specialty.SpecialtyId);
            Assert.Equal(EntityStatus.Active, stored!.Status);

            Assert.Equal(2, stored!.Treatments.Count);

            Assert.True(stored.Treatments.Single(t => 
            t.TreatmentId == braces.TreatmentId).Status.IsActive);

            Assert.True(stored.Treatments.Single(t => 
            t.TreatmentId == aligners.TreatmentId).Status.IsInactive);

            Assert.True(repository.SaveWasCalled);
        }


        // Errors

        [Fact]
        public async Task Handle_WhenSpecialtyIsInactive_ShouldThrowInvalidSpecialtyStateException()
        {
            // Arrange
            var aligners = TreatmentBuilder.Active(name: "Aligners", cost: 20, description: "Other treament");
            var braces = TreatmentBuilder.Active();
            Specialty specialty = SpecialtyBuilder.ActiveWithTreatments(braces, aligners);
            specialty.Deactivate();

            var repository = new FakeSpecialtyRepository();
            repository.Add(specialty);

            var command = DeactivateTreatmentCommandBuilder.Valid(specialty.SpecialtyId, aligners.TreatmentId);

            var handler = new DeactivateTreatmentHandler(repository);

            // Act
            // Assert
            await Assert.ThrowsAnyAsync<DomainException>(async () =>
            {
                await handler.Handle(command);
            });
            Assert.False(repository.SaveWasCalled);
        }


        [Fact]
        public async Task Handle_WhenDeactivateTheLastTreatment_ShouldThrowMinimunSpecialtyTeatmentsException()
        {
            // Arrange
            var braces = TreatmentBuilder.Active();
            Specialty specialty = SpecialtyBuilder.ActiveWithTreatments(braces);

            var repository = new FakeSpecialtyRepository();
            repository.Add(specialty);
            var handler = new DeactivateTreatmentHandler(repository);

            var command = DeactivateTreatmentCommandBuilder.Valid(specialty.SpecialtyId, braces.TreatmentId);

            // Act
            // Assert
            await Assert.ThrowsAnyAsync<DomainException>(async () =>
            {
                await handler.Handle(command);
            });
            var stored = await repository.GetById(specialty.SpecialtyId);
            Assert.Equal(EntityStatus.Active, stored!.Status);
            Assert.NotNull(stored);
            Assert.True(stored.Treatments.Single(t =>
                 t.TreatmentId == braces.TreatmentId).Status.IsActive);
            Assert.False(repository.SaveWasCalled);
        }
    }
}
