using DentalSystem.Application.Exceptions;
using DentalSystem.Application.Tests.Builders.Domain.Specialties;
using DentalSystem.Application.Tests.Fakes.Persistence;
using DentalSystem.Application.Tests.Fakes.Repositories.Specialties;
using DentalSystem.Application.UseCases.Specialties.EditTreatment.CorrectTreatmentName;
using DentalSystem.Domain.Entities;
using DentalSystem.Domain.Exceptions.Specialties;

namespace DentalSystem.Application.Tests.UseCases.Specialties.EditTreatmentTests.CorrectTreatmentNameTests
{
    public sealed class CorrectTreatmentNameTest
    {
        // Happy path
        [Fact]
        public async Task Handle_WhenNameIsValidAndDifferent_ShouldCommit()
        {
            // Arrange
            string expectedName = "Braces Sp";
            Specialty specialty = SpecialtyBuilder.CreateActiveWithOneTreatment();

            FakeUnitOfWork unitOfWork = new();
            FakeSpecialtyRepository repository = new(unitOfWork);

            repository.Add(specialty);

            Guid treatmentId = specialty.Treatments.First().TreatmentId;

            CorrectTreatmentNameCommand command = new(specialty.SpecialtyId, treatmentId, expectedName);
            CorrectTreatmentNameHandler handler = new(repository, unitOfWork);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            var stored = await repository.GetById(specialty.SpecialtyId, CancellationToken.None);
            Assert.Equal(expectedName, stored!.Treatments.Single(t =>
                t.TreatmentId == treatmentId).Name.Value);
            Assert.True(unitOfWork.WasCommitted);
        }

       
        // Name is the same
        [Fact]
        public async Task Handle_WhenNameIsSame_ShouldNotCommit()
        {
            // Arrange
            Specialty specialty = SpecialtyBuilder.CreateActiveWithOneTreatment();

            FakeUnitOfWork unitOfWork = new();
            FakeSpecialtyRepository repository = new(unitOfWork);

            repository.Add(specialty);

            Guid treatmentId = specialty.Treatments.First().TreatmentId;

            CorrectTreatmentNameCommand command = new(specialty.SpecialtyId, treatmentId, "Braces");
            CorrectTreatmentNameHandler handler = new(repository, unitOfWork);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(unitOfWork.WasCommitted);
        }

        // specialty does not exist
        [Fact]
        public async Task Handle_WhenSpecialtyDoesNotExist_ShouldThrowSpecialtyNotFoundExceptionAndNotCommit()
        {
            // Arrange
            FakeUnitOfWork unitOfWork = new();
            FakeSpecialtyRepository repository = new(unitOfWork);

            CorrectTreatmentNameCommand command = new(Guid.NewGuid(), Guid.NewGuid(), "Braces");
            CorrectTreatmentNameHandler handler = new(repository, unitOfWork);

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
            treatment.Deactivate();

            CorrectTreatmentNameCommand command = new(specialty.SpecialtyId, treatment.TreatmentId, "Braces Sp");
            CorrectTreatmentNameHandler handler = new(repository, unitOfWork);

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

            CorrectTreatmentNameCommand command = new(specialty.SpecialtyId, Guid.NewGuid(), "Braces Sp");
            CorrectTreatmentNameHandler handler = new(repository, unitOfWork);

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
