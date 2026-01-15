using DentalSystem.Application.Exceptions;
using DentalSystem.Application.Tests.Builders.Domain.Specialties;
using DentalSystem.Application.Tests.Fakes.Persistence;
using DentalSystem.Application.Tests.Fakes.Repositories.Specialties;
using DentalSystem.Application.UseCases.Specialties.ReactivateTreatment;
using DentalSystem.Domain.Entities;
using DentalSystem.Domain.Exceptions.Specialties;

namespace DentalSystem.Application.Tests.UseCases.Specialties.ReactivateTreatmentTests
{
    public sealed class ReactivateTreatmentTest
    {
        // Happy path
        [Fact]
        public async Task Handle_WhenTreatmentIsInactive_ShouldReactivateTreatmentAndCommit()
        {
            // Arrange
            Specialty specialty = SpecialtyBuilder.CreateActiveWithTwoDistinctTreatments();
            Treatment treatment = specialty.Treatments.First();

            specialty.DeactivateTreatment(treatment.TreatmentId);

            FakeUnitOfWork unitOfWork = new();
            FakeSpecialtyRepository repository = new(unitOfWork);
            repository.Add(specialty);

            ReactivateTreatmentCommand command = new(specialty.SpecialtyId, treatment.TreatmentId);

            ReactivateTreatmentHandler handler = new(repository, unitOfWork);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            Specialty stored = (await repository.GetById(
                specialty.SpecialtyId, CancellationToken.None))!;

            Treatment reactivated =
                stored.Treatments.Single(t => t.TreatmentId == treatment.TreatmentId);

            Assert.True(reactivated.Status.IsActive);
            Assert.True(unitOfWork.WasCommitted);
        }


        // Specialty not found
        [Fact]
        public async Task Handle_WhenSpecialtyDoesNotExist_ShouldThrowSpecialtyNotFoundException()
        {
            // Arrange
            FakeUnitOfWork unitOfWork = new();
            FakeSpecialtyRepository repository = new(unitOfWork);

            ReactivateTreatmentCommand command = new(Guid.NewGuid(), Guid.NewGuid());

            ReactivateTreatmentHandler handler = new(repository, unitOfWork);
            // Act
            // Assert
            await Assert.ThrowsAsync<SpecialtyNotFoundException>(() =>
                handler.Handle(command, CancellationToken.None));

            Assert.False(unitOfWork.WasCommitted);
        }

        //Specialty inactive
        [Fact]
        public async Task Handle_WhenSpecialtyIsInactive_ShouldThrowInvalidSpecialtyStateException()
        {
            // Arrange
            Specialty specialty = SpecialtyBuilder.CreateInactive();
            Treatment treatment = specialty.Treatments.First();

            FakeUnitOfWork unitOfWork = new();
            FakeSpecialtyRepository repository = new(unitOfWork);
            repository.Add(specialty);

            ReactivateTreatmentCommand command =new(specialty.SpecialtyId, treatment.TreatmentId);

            ReactivateTreatmentHandler handler =new(repository, unitOfWork);

            // Act
            // Assert
            await Assert.ThrowsAsync<InvalidSpecialtyStateException>(() =>
                handler.Handle(command, CancellationToken.None));

            Assert.False(unitOfWork.WasCommitted);
        }

        //Treatment not found
        [Fact]
        public async Task Handle_WhenTreatmentDoesNotExist_ShouldThrowTreatmentNotFoundException()
        {
            // Arrange
            Specialty specialty = SpecialtyBuilder.CreateActiveWithOneTreatment();

            FakeUnitOfWork unitOfWork = new();
            FakeSpecialtyRepository repository = new(unitOfWork);
            repository.Add(specialty);

            ReactivateTreatmentCommand command = new(specialty.SpecialtyId, Guid.NewGuid());

            ReactivateTreatmentHandler handler = new(repository, unitOfWork);

            // Act
            // Assert
            await Assert.ThrowsAsync<TreatmentNotFoundException>(() =>
                handler.Handle(command, CancellationToken.None));

            Assert.False(unitOfWork.WasCommitted);
        }

        //Treatment already active
        [Fact]
        public async Task Handle_WhenTreatmentIsAlreadyActive_ShouldThrowInvalidStatusTransitionException()
        {
            // Arrange
            Specialty specialty = SpecialtyBuilder.CreateActiveWithOneTreatment();
            Treatment treatment = specialty.Treatments.First();

            FakeUnitOfWork unitOfWork = new();
            FakeSpecialtyRepository repository = new(unitOfWork);
            repository.Add(specialty);

            ReactivateTreatmentCommand command = new(specialty.SpecialtyId, treatment.TreatmentId);

            ReactivateTreatmentHandler handler = new(repository, unitOfWork);

            // Act
            // Assert
            await Assert.ThrowsAsync<InvalidStatusTransitionException>(() =>
                handler.Handle(command, CancellationToken.None));

            Assert.False(unitOfWork.WasCommitted);
        }

    }
}
