using DentalSystem.Application.Exceptions;
using DentalSystem.Application.Tests.Builders.Domain.Specialties;
using DentalSystem.Application.Tests.Fakes.Persistence;
using DentalSystem.Application.Tests.Fakes.Repositories.Specialties;
using DentalSystem.Application.UseCases.Specialties.EditTreatment.ChangeTreatmentCost;
using DentalSystem.Application.UseCases.Specialties.EditTreatment.UpdateTreatmentDescription;
using DentalSystem.Domain.Entities;
using DentalSystem.Domain.Exceptions.Specialties;
using DentalSystem.Domain.Exceptions.ValueObjects;

namespace DentalSystem.Application.Tests.UseCases.Specialties.EditTreatmentTests.UpdateTreatmentDescriptionTests
{
    public sealed class UpdateTreatmentDescriptionTest
    {
        // happy path
        [Theory]
        [InlineData("Updated description")]
        [InlineData(null)]
        public async Task Handle_WhenInputDataIsValid_ShouldChangeTreatmentDescriptionAndPersist(string? updatedDescription)
        {
            // Arrange
            Specialty specialty = SpecialtyBuilder.CreateActiveWithOneTreatment();

            FakeUnitOfWork unitOfWork = new();
            FakeSpecialtyRepository repository = new(unitOfWork);

            repository.Add(specialty);

            Guid treatmentId = specialty.Treatments.First().TreatmentId;

            UpdateTreatmentDescriptionCommand command = new(specialty.SpecialtyId, treatmentId, updatedDescription);
            UpdateTreatmentDescriptionHandler handler = new(repository, unitOfWork);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            var stored = await repository.GetById(specialty.SpecialtyId, CancellationToken.None);
            var updatedTreatment = stored!.Treatments.Single(t => t.TreatmentId == treatmentId);
            Assert.Equal(updatedDescription, updatedTreatment.Description?.Value);
            Assert.True(unitOfWork.WasCommitted);
        }



        // Errors
        [Fact]
        public async Task Handle_WhenDescriptionIsInvalid_ShouldThrowInvalidDescriptionException_AndNotPersist()
        {
            // Arrange
            Specialty specialty = SpecialtyBuilder.CreateActiveWithOneTreatment();

            FakeUnitOfWork unitOfWork = new();
            FakeSpecialtyRepository repository = new(unitOfWork);

            repository.Add(specialty);

            Guid treatmentId = specialty.Treatments.First().TreatmentId;

            UpdateTreatmentDescriptionCommand command = new(specialty.SpecialtyId, treatmentId, "!!");
            UpdateTreatmentDescriptionHandler handler = new(repository, unitOfWork);

            // Act
            // Assert
            await Assert.ThrowsAsync<InvalidDescriptionException>(async () =>
            {
                await handler.Handle(command, CancellationToken.None);
            });
            Assert.False(unitOfWork.WasCommitted);
        }


        [Fact]
        public async Task Handle_WhenSpecialtyIsInactive_ShouldThrowInvalidSpecialtyStateException_AndNotPersist()
        {
            // Arrange
            Specialty specialty = SpecialtyBuilder.CreateInactive();

            FakeUnitOfWork unitOfWork = new();
            FakeSpecialtyRepository repository = new(unitOfWork);

            repository.Add(specialty);

            Guid treatmentId = specialty.Treatments.First().TreatmentId;

            UpdateTreatmentDescriptionCommand command = new(specialty.SpecialtyId, treatmentId, "!!");
            UpdateTreatmentDescriptionHandler handler = new(repository, unitOfWork);

            // Act
            // Assert
            await Assert.ThrowsAsync<InvalidSpecialtyStateException>(async () =>
            {
                await handler.Handle(command, CancellationToken.None);
            });
            Assert.False(unitOfWork.WasCommitted);
        }


        [Fact]
        public async Task Handle_WhenSpecialtyDoesNotExist_ShouldThrowSpecialtyNotFoundExceptionAndNotCommit()
        {
            // Arrange
            FakeUnitOfWork unitOfWork = new();
            FakeSpecialtyRepository repository = new(unitOfWork);

            UpdateTreatmentDescriptionCommand command = new(Guid.NewGuid(), Guid.NewGuid(), "Updated description");
            UpdateTreatmentDescriptionHandler handler = new(repository, unitOfWork);

            // Act
            // Assert
            await Assert.ThrowsAsync<SpecialtyNotFoundException>(async () =>
            {
                await handler.Handle(command, CancellationToken.None);
            });

            Assert.False(unitOfWork.WasCommitted);
        }


        [Fact]
        public async Task Handle_WhenTreatmentIsInactive_ShouldThrowInvalidTreatmentStateExceptionAndNotCommit()
        {
            // Arrange
            Specialty specialty = SpecialtyBuilder.CreateActiveWithTwoDistinctTreatments();

            FakeUnitOfWork unitOfWork = new();
            FakeSpecialtyRepository repository = new(unitOfWork);

            repository.Add(specialty);

            Treatment treatment = specialty.Treatments.First();
            specialty.DeactivateTreatment(treatment.TreatmentId);

            UpdateTreatmentDescriptionCommand command = new(specialty.SpecialtyId, treatment.TreatmentId, "Updated description");
            UpdateTreatmentDescriptionHandler handler = new(repository, unitOfWork);

            // Act
            // Assert
            await Assert.ThrowsAsync<InvalidTreatmentStateException>(async () =>
            {
                await handler.Handle(command, CancellationToken.None);
            });

            Assert.False(unitOfWork.WasCommitted);
        }


        [Fact]
        public async Task Handle_WhenTreatmentDoesNotExist_ShouldThrowTreatmentNotFoundExceptionAndNotCommit()
        {
            // Arrange
            Specialty specialty = SpecialtyBuilder.CreateActiveWithTwoDistinctTreatments();

            FakeUnitOfWork unitOfWork = new();
            FakeSpecialtyRepository repository = new(unitOfWork);

            repository.Add(specialty);

            UpdateTreatmentDescriptionCommand command = new(specialty.SpecialtyId, Guid.NewGuid(), "Updated description");
            UpdateTreatmentDescriptionHandler handler = new(repository, unitOfWork);

            // Act
            // Assert
            await Assert.ThrowsAsync<TreatmentNotFoundException>(async () =>
            {
                await handler.Handle(command, CancellationToken.None);
            });

            Assert.False(unitOfWork.WasCommitted);
        }

    }
}
