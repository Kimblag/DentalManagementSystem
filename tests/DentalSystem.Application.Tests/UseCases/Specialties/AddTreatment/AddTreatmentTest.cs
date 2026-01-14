using DentalSystem.Application.Tests.Builders.Commands.Specialties;
using DentalSystem.Application.Tests.Builders.Domain.Specialties;
using DentalSystem.Application.Tests.Fakes.Repositories.Specialties;
using DentalSystem.Application.UseCases.Specialties.AddTreatment;
using DentalSystem.Domain.Entities;
using DentalSystem.Domain.Exceptions;

namespace DentalSystem.Application.Tests.UseCases.Specialties.AddTreatment
{
    public class AddTreatmentTest
    {
        // Happy path
        [Fact]
        public async Task Handle_WhenTreatmentInputIsValid_ShouldAddTreatmentToSpecialtyAndPersistSpecialty()
        {
            // Arrange
            string newTreatmentName = "Aligners";

            Specialty specialty = SpecialtyBuilder.ActiveWithOneTreatment();

            var repository = new FakeSpecialtyRepository();
            repository.Add(specialty);

            var command = AddTreatmentCommandBuilder.WithName(specialty.SpecialtyId, newTreatmentName);
            var handler = new AddTreatmentHandler(repository);

            // Act
            await handler.Handle(command);

            // Assert
            var stored = await repository.GetById(specialty.SpecialtyId);

            // Check if specialty has the new treatment
            Assert.Equal(2, stored!.Treatments.Count);

            var addedTreatment = stored.Treatments.First(t =>
                t.Name.Value == newTreatmentName);

            Assert.True(addedTreatment.Status.IsActive);
            // IT must be persisted
            Assert.True(repository.SaveWasCalled);
        }


        // Errors
        [Fact]
        public async Task Handle_WhenSpecialtyIsInactive_ShouldThrowInvalidSpecialtyStateException()
        {
            // Arrange
            Specialty specialty = SpecialtyBuilder.ActiveWithOneTreatment();
            specialty.Deactivate();
            
            var repository = new FakeSpecialtyRepository();
            repository.Add(specialty);

            var command = AddTreatmentCommandBuilder.Valid(specialty.SpecialtyId);
            var handler = new AddTreatmentHandler(repository);

            // Act
            // Assert
            await Assert.ThrowsAnyAsync<DomainException>(async () =>
            {
                await handler.Handle(command);
            });

            var storedInFakeSpecialty = await repository.GetById(specialty.SpecialtyId);
            Assert.NotNull(storedInFakeSpecialty);
            Assert.Single(storedInFakeSpecialty.Treatments);
            Assert.False(repository.SaveWasCalled);
        }


        [Fact]
        public async Task Handle_WhenTreatmentAlreadyExists_ShouldThrowDuplicateTreatmentNameException()
        {
            // Arrange
            Specialty specialty = SpecialtyBuilder.ActiveWithOneTreatment();

            var repository = new FakeSpecialtyRepository();
            repository.Add(specialty);

            var command = AddTreatmentCommandBuilder.WithName(specialty.SpecialtyId, "Braces");
            var handler = new AddTreatmentHandler(repository);

            // Act
            // Assert
            await Assert.ThrowsAnyAsync<DomainException>(async () =>
            {
                await handler.Handle(command);
            });
            var storedInFakeSpecialty = await repository.GetById(specialty.SpecialtyId);
            Assert.NotNull(storedInFakeSpecialty);
            Assert.Single(storedInFakeSpecialty.Treatments);
            Assert.False(repository.SaveWasCalled);
        }
    }
}
