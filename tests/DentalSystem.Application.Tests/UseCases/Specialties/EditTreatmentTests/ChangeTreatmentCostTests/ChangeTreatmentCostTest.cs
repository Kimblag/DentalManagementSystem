using DentalSystem.Application.Exceptions;
using DentalSystem.Application.Tests.Builders.Domain.Specialties;
using DentalSystem.Application.Tests.Fakes.Persistence;
using DentalSystem.Application.Tests.Fakes.Repositories.Specialties;
using DentalSystem.Application.UseCases.Specialties.EditTreatment.ChangeTreatmentCost;
using DentalSystem.Domain.Entities;
using DentalSystem.Domain.Exceptions.Rules.Specialties;

namespace DentalSystem.Application.Tests.UseCases.Specialties.EditTreatmentTests.ChangeTreatmentCostTests
{
    public class ChangeTreatmentCostTest
    {
        // happy path
        [Fact]
        public async Task Handle_WhenCostIsValidAndDifferent_ShouldCommit()
        {
            // Arrange
            Specialty specialty = SpecialtyBuilder.CreateActiveWithOneTreatment();

            FakeUnitOfWork unitOfWork = new();
            FakeSpecialtyRepository repository = new();

            repository.Add(specialty);

            Guid treatmentId = specialty.Treatments.First().TreatmentId;

            ChangeTreatmentCostCommand command = new(specialty.SpecialtyId, treatmentId, 30);
            ChangeTreatmentCostHandler handler = new(repository, unitOfWork);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            var stored = await repository.GetByIdAsync(specialty.SpecialtyId, CancellationToken.None);
            Assert.Equal(30, stored!.Treatments.Single(t =>
                t.TreatmentId == treatmentId).BaseCost);
        }

        // Errors
        [Fact]
        public async Task Handle_WhenSpecialtyDoesNotExist_ShouldThrowSpecialtyNotFoundExceptionAndNotCommit()
        {
            // Arrange
            FakeUnitOfWork unitOfWork = new();
            FakeSpecialtyRepository repository = new();

            ChangeTreatmentCostCommand command = new(Guid.NewGuid(), Guid.NewGuid(), 30);
            ChangeTreatmentCostHandler handler = new(repository, unitOfWork);

            // Act
            // Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await handler.Handle(command, CancellationToken.None);
            });

        }


        [Fact]
        public async Task Handle_WhenCostIsSame_ShouldLeaveAggregateUnchanged()
        {
            // Arrange
            Specialty specialty = SpecialtyBuilder.CreateActiveWithOneTreatment();

            FakeUnitOfWork unitOfWork = new();
            FakeSpecialtyRepository repository = new();

            repository.Add(specialty);

            var treatment = specialty.Treatments.First();
            decimal originalBaseCost = treatment.BaseCost;

            ChangeTreatmentCostCommand command = new(specialty.SpecialtyId, treatment.TreatmentId, treatment.BaseCost);
            ChangeTreatmentCostHandler handler = new(repository, unitOfWork);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            Specialty? unchangedSpecialty = await repository.GetByIdAsync(specialty.SpecialtyId, CancellationToken.None);
            var unchangedTreatment = unchangedSpecialty!.Treatments.First();
            Assert.Equal(originalBaseCost, unchangedTreatment.BaseCost);
        }


        [Fact]
        public async Task Handle_WhenTreatmentIsInactive_ShouldThrowInvalidTreatmentStateExceptionAndNotCommit()
        {
            // Arrange
            Specialty specialty = SpecialtyBuilder.CreateActiveWithTwoDistinctTreatments();

            FakeUnitOfWork unitOfWork = new();
            FakeSpecialtyRepository repository = new();

            repository.Add(specialty);

            Treatment treatment = specialty.Treatments.First();
            specialty.DeactivateTreatment(treatment.TreatmentId);

            ChangeTreatmentCostCommand command = new(specialty.SpecialtyId, treatment.TreatmentId, 10);
            ChangeTreatmentCostHandler handler = new(repository, unitOfWork);

            // Act
            // Assert
            await Assert.ThrowsAsync<InvalidTreatmentStateException>(async () =>
            {
                await handler.Handle(command, CancellationToken.None);
            });
        }


        [Fact]
        public async Task Handle_WhenTreatmentDoesNotExist_ShouldThrowTreatmentNotFoundExceptionAndNotCommit()
        {
            // Arrange
            Specialty specialty = SpecialtyBuilder.CreateActiveWithTwoDistinctTreatments();

            FakeUnitOfWork unitOfWork = new();
            FakeSpecialtyRepository repository = new();

            repository.Add(specialty);

            ChangeTreatmentCostCommand command = new(specialty.SpecialtyId, Guid.NewGuid(), 30);
            ChangeTreatmentCostHandler handler = new(repository, unitOfWork);

            // Act
            // Assert
            await Assert.ThrowsAsync<TreatmentNotFoundException>(async () =>
            {
                await handler.Handle(command, CancellationToken.None);
            });
        }
    }
}
