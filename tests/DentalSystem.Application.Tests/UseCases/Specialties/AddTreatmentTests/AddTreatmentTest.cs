using DentalSystem.Application.Contracts.Specialties;
using DentalSystem.Application.Tests.Builders.Domain.Specialties;
using DentalSystem.Application.Tests.Fakes.Persistence;
using DentalSystem.Application.Tests.Fakes.Repositories.Specialties;
using DentalSystem.Application.UseCases.Specialties.AddTreatment;
using DentalSystem.Domain.Entities;
using DentalSystem.Domain.Exceptions.Rules.Specialties;

namespace DentalSystem.Application.Tests.UseCases.Specialties.AddTreatmentTests
{
    public sealed class AddTreatmentTest
    {
        // Happy path
        [Fact]
        public async Task Handle_WhenTreatmentInputIsValid_ShouldAddTreatmentToSpecialtyAndPersistSpecialty()
        {
            // Arrange
            Specialty specialty = SpecialtyBuilder.CreateActiveWithOneTreatment();

            FakeUnitOfWork unitOfWork = new();
            FakeSpecialtyRepository repository = new();
            repository.Add(specialty);

            TreatmentInput treatmentInput = new("Aligners", 20, "Clear aligners");
            AddTreatmentCommand command = new(specialty.SpecialtyId, treatmentInput);

            AddTreatmentHandler handler = new(repository, unitOfWork);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            var stored = await repository.GetByIdAsync(specialty.SpecialtyId, CancellationToken.None);
            Assert.Equal(2, stored!.Treatments.Count);
            Assert.Contains(stored.Treatments, t => t.Name.Value == "Aligners");
        }


        // Errors
        [Fact]
        public async Task Handle_WhenSpecialtyIsInactive_ShouldThrowInvalidSpecialtyStateException()
        {
            // Arrange
            Specialty specialty = SpecialtyBuilder.CreateInactive();

            FakeUnitOfWork unitOfWork = new();
            FakeSpecialtyRepository repository = new();
            repository.Add(specialty);

            TreatmentInput treatmentInput = new("Aligners", 20, "Clear aligners");
            AddTreatmentCommand command = new(specialty.SpecialtyId, treatmentInput);

            AddTreatmentHandler handler = new(repository, unitOfWork);

            // Act
            // Assert
            await Assert.ThrowsAsync<InvalidSpecialtyStateException>(async () =>
            {
                await handler.Handle(command, CancellationToken.None);
            });
        }


        [Fact]
        public async Task Handle_WhenTreatmentAlreadyExists_ShouldThrowDuplicateTreatmentNameException()
        {
            // Arrange
            Specialty specialty = SpecialtyBuilder.CreateActiveWithOneTreatment();

            FakeUnitOfWork unitOfWork = new();
            FakeSpecialtyRepository repository = new();
            repository.Add(specialty);

            TreatmentInput treatmentInput = new("Braces", 10, "Description of a treatment");
            AddTreatmentCommand command = new(specialty.SpecialtyId, treatmentInput);

            AddTreatmentHandler handler = new(repository, unitOfWork);

            // Act
            // Assert
            await Assert.ThrowsAsync<DuplicateTreatmentNameException>(async () =>
            {
                await handler.Handle(command, CancellationToken.None);
            });
        }
    }
}
