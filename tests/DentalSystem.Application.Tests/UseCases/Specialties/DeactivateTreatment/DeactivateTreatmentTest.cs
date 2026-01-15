using DentalSystem.Application.Tests.Builders.Domain.Specialties;
using DentalSystem.Application.Tests.Fakes.Persistence;
using DentalSystem.Application.Tests.Fakes.Repositories.Specialties;
using DentalSystem.Application.UseCases.Specialties.DeactivateTreatment;
using DentalSystem.Domain.Entities;
using DentalSystem.Domain.Exceptions.Specialties;

namespace DentalSystem.Application.Tests.UseCases.Specialties.DeactivateTreatment
{
    public sealed class DeactivateTreatmentTest
    {
        // Happy path
        [Fact]
        public async Task Handle_WhenDeactivateAnExistingTreatment_ShouldDeactivateInSpecialtyAndPersistChanges()
        {
            // Arrange
            Specialty specialty = SpecialtyBuilder.CreateActiveWithTwoDistinctTreatments();
            Guid treatmentToDeactivateId = specialty.Treatments.First().TreatmentId;

            DeactivateTreatmentCommand command = new(specialty.SpecialtyId, treatmentToDeactivateId);

            FakeUnitOfWork unitOfWork = new();
            FakeSpecialtyRepository repository = new(unitOfWork);
            repository.Add(specialty);

            DeactivateTreatmentHandler handler = new(repository, unitOfWork);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            var stored = await repository.GetById(specialty.SpecialtyId, CancellationToken.None);
            Assert.True(stored!.Treatments.Single(t => 
                t.TreatmentId == treatmentToDeactivateId).Status.IsInactive);
            Assert.True(unitOfWork.WasCommitted);
        }


        // Errors

        [Fact]
        public async Task Handle_WhenSpecialtyIsInactive_ShouldThrowInvalidSpecialtyStateException()
        {
            // Arrange
            Specialty specialty = SpecialtyBuilder.CreateInactive();
            Guid treatmentToDeactivateId = specialty.Treatments.First().TreatmentId;

            DeactivateTreatmentCommand command = new(specialty.SpecialtyId, treatmentToDeactivateId);

            FakeUnitOfWork unitOfWork = new();
            FakeSpecialtyRepository repository = new(unitOfWork);
            repository.Add(specialty);

            DeactivateTreatmentHandler handler = new(repository, unitOfWork);

            // Act
            // Assert
            await Assert.ThrowsAsync<InvalidSpecialtyStateException>(async () =>
            {
                await handler.Handle(command, CancellationToken.None);
            });

            Assert.False(unitOfWork.WasCommitted);
        }


        [Fact]
        public async Task Handle_WhenDeactivateTheLastTreatment_ShouldThrowMinimumSpecialtyTreatmentsException()
        {
            // Arrange
            Specialty specialty = SpecialtyBuilder.CreateActiveWithOneTreatment();
            Guid treatmentToDeactivateId = specialty.Treatments.First().TreatmentId;

            DeactivateTreatmentCommand command = new(specialty.SpecialtyId, treatmentToDeactivateId);

            FakeUnitOfWork unitOfWork = new();
            FakeSpecialtyRepository repository = new(unitOfWork);
            repository.Add(specialty);

            DeactivateTreatmentHandler handler = new(repository, unitOfWork);

            // Act
            // Assert
            await Assert.ThrowsAsync<MinimumSpecialtyTreatmentsException>(async () =>
            {
                await handler.Handle(command, CancellationToken.None);
            });

            Assert.False(unitOfWork.WasCommitted);
        }


        [Fact]
        public async Task Handle_WhenTreatmentDoesNotExist_ShouldThrowTreatmentNotFoundException()
        {
            // Arrange
            Specialty specialty = SpecialtyBuilder.CreateActiveWithTwoDistinctTreatments();
            
            DeactivateTreatmentCommand command = new(specialty.SpecialtyId, Guid.NewGuid());

            FakeUnitOfWork unitOfWork = new();
            FakeSpecialtyRepository repository = new(unitOfWork);
            repository.Add(specialty);

            DeactivateTreatmentHandler handler = new(repository, unitOfWork);

            // Act
            // Assert
            await Assert.ThrowsAsync<TreatmentNotFoundException>(async () =>
            {
                await handler.Handle(command, CancellationToken.None);
            });

            Assert.False(unitOfWork.WasCommitted);
        }


        [Fact]
        public async Task Handle_WhenTreatmentIsAlreadyInactive_ShouldThrowInvalidStatusTransitionException()
        {
            // Arrange
            Specialty specialty = SpecialtyBuilder.CreateActiveWithTwoDistinctTreatments();
            Guid treatmentToDeactivateId = specialty.Treatments.First().TreatmentId;
            Treatment treatmentToDeactivate = specialty.Treatments.First();
            treatmentToDeactivate.Deactivate();

            DeactivateTreatmentCommand command = new(specialty.SpecialtyId, treatmentToDeactivateId);

            FakeUnitOfWork unitOfWork = new();
            FakeSpecialtyRepository repository = new(unitOfWork);
            repository.Add(specialty);

            DeactivateTreatmentHandler handler = new(repository, unitOfWork);

            // Act
            // Assert
            await Assert.ThrowsAsync<InvalidStatusTransitionException>(async () =>
            {
                await handler.Handle(command, CancellationToken.None);
            });

            Assert.False(unitOfWork.WasCommitted);
        }
    }
}
