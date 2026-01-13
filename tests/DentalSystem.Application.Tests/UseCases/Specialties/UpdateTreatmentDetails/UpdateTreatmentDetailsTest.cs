
using DentalSystem.Application.Tests.Builders.Commands.Specialties;
using DentalSystem.Application.Tests.Builders.Domain.Specialties;
using DentalSystem.Application.Tests.Fakes.Repositories.Specialties;
using DentalSystem.Application.UseCases.Specialties.UpdateTreatmentDetails;
using DentalSystem.Domain.Entities;
using DentalSystem.Domain.Exceptions;
using DentalSystem.Domain.Exceptions.Specialties;
using System.Globalization;

namespace DentalSystem.Application.Tests.UseCases.Specialties.UpdateTreatmentDetails
{
    public class UpdateTreatmentDetailsTest
    {
        // Happy path
        [Fact]
        public async Task Handle_WhenInputDataForTreatmentDetailsIsValid_ShouldUpdateTreatmentDetails_AndPersist()
        {
            // Arrange
            var braces = TreatmentBuilder.Active("Brace");
            var aligners = TreatmentBuilder.Active("Aligners", 20, "Other treatment");

            Specialty specialty = SpecialtyBuilder.ActiveWithTreatments(braces, aligners);

            var repository = new FakeSpecialtyRepository();
            repository.Add(specialty);

            var command = UpdateTreatmentDetailsCommandBuilder.Valid(specialty.SpecialtyId, braces.TreatmentId);


            var handler = new UpdateTreatmentDetailsHandler(repository);

            // Act
            await handler.Handle(command);

            // Assert
            var storedInFakeSpecialty = await repository.GetById(specialty.SpecialtyId);
            Assert.NotNull(storedInFakeSpecialty);
            var addedTreatment = storedInFakeSpecialty.Treatments.First(t =>
               t.TreatmentId.Equals(braces.TreatmentId));
            Assert.Equal(command.TreatmentName, addedTreatment.Name);
            Assert.Equal(command.TreatmentBaseCost, addedTreatment.BaseCost);
            Assert.Equal(command.TreatmentDescription, addedTreatment.Description);
            Assert.True(repository.SaveWasCalled);
        }


        // Errors
        // Attempt to update treatment of an inactive specialty
        [Fact]
        public async Task Handle_WhenTreatmentIsWithinInactiveSpecialty_ShouldThrowInvalidSpecialtyStateException()
        {
            // Arrange
            string oldTreatmentName = "Brace";
            string oldDescription = "A description";
            decimal oldBaseCost = 10;

            var braces = TreatmentBuilder.Active(oldTreatmentName, oldBaseCost, oldDescription);

            Specialty specialty = SpecialtyBuilder.ActiveWithTreatments(braces);
            specialty.Deactivate();

            var repository = new FakeSpecialtyRepository();
            repository.Add(specialty);
            var handler = new UpdateTreatmentDetailsHandler(repository);

            var command = UpdateTreatmentDetailsCommandBuilder.Valid(specialty.SpecialtyId, braces.TreatmentId);

            // Act
            // Assert
            await Assert.ThrowsAnyAsync<DomainException>(async () =>
            {
                await handler.Handle(command);
            });

            // Check that the treatment did not change
            var storedInFakeSpecialty = await repository.GetById(specialty.SpecialtyId);
            Assert.NotNull(storedInFakeSpecialty);
            
            var addedTreatment = storedInFakeSpecialty.Treatments.First(t =>
               t.TreatmentId.Equals(braces.TreatmentId));
            
            Assert.Equal(oldTreatmentName, addedTreatment.Name);
            Assert.Equal(oldBaseCost, addedTreatment.BaseCost);
            Assert.Equal(oldDescription, addedTreatment.Description);
            Assert.False(repository.SaveWasCalled);
        }


        //  Attempt to update an inactive treatment
        [Fact]
        public async Task Handle_WhenTreatmentIsInactive_ShouldThrowInvalidTreatmentStateException()
        {
            // Arrange
            string oldTreatmentName = "Brace";
            string oldDescription = "A description";
            decimal oldBaseCost = 10;

            var braces = TreatmentBuilder.Active(oldTreatmentName, oldBaseCost, oldDescription);
            var aligners = TreatmentBuilder.Active("Aligners", 20, "Other treatment");

            Specialty specialty = SpecialtyBuilder.ActiveWithTreatments(braces, aligners);
            specialty.DeactivateTreatment(braces.TreatmentId);

            var repository = new FakeSpecialtyRepository();
            repository.Add(specialty);
            var handler = new UpdateTreatmentDetailsHandler(repository);

            var command = UpdateTreatmentDetailsCommandBuilder.Valid(specialty.SpecialtyId, braces.TreatmentId);

            // Act
            // Assert
            await Assert.ThrowsAnyAsync<DomainException>(async () =>
            {
                await handler.Handle(command);
            });

            // Check that nothing did not change
            var storedInFakeSpecialty = await repository.GetById(specialty.SpecialtyId);
            Assert.NotNull(storedInFakeSpecialty);

            var addedTreatment = storedInFakeSpecialty.Treatments.First(t =>
               t.TreatmentId.Equals(braces.TreatmentId));

            Assert.Equal(oldTreatmentName, addedTreatment.Name);
            Assert.Equal(oldBaseCost, addedTreatment.BaseCost);
            Assert.Equal(oldDescription, addedTreatment.Description);
            Assert.False(repository.SaveWasCalled);
        }


        // Attempt to update a non-existent treatment
        [Fact]
        public async Task Handle_WhenNonExistentTreatment_ShouldThrowTreatmentNotFoundException()
        {
            // Arrange
            Specialty specialty = SpecialtyBuilder.ActiveWithOneTreatment();

            var repository = new FakeSpecialtyRepository();
            repository.Add(specialty);
            var handler = new UpdateTreatmentDetailsHandler(repository);

            var command = UpdateTreatmentDetailsCommandBuilder.Valid(specialty.SpecialtyId, Guid.NewGuid());

            // Act
            // Assert
            await Assert.ThrowsAnyAsync<DomainException>(async () =>
            {
                await handler.Handle(command);
            });         
            Assert.False(repository.SaveWasCalled);
        }


        // Attempt to update treatment with invalid data
        [Fact]
        public async Task Handle_WhenTreatmentDetailsAreInvalid_ShouldThrowDomainException()
        {
            // Arrange
            string oldTreatmentName = "Brace";
            string oldTreatmentDescription = "Old description";
            decimal oldTreatmentBaseCost = 10;

            var braces = TreatmentBuilder.Active(oldTreatmentName, oldTreatmentBaseCost, oldTreatmentDescription);
            Specialty specialty = SpecialtyBuilder.ActiveWithTreatments(braces);


            var repository = new FakeSpecialtyRepository();
            repository.Add(specialty);
            var handler = new UpdateTreatmentDetailsHandler(repository);

            var command = UpdateTreatmentDetailsCommandBuilder.Invalid(specialty.SpecialtyId, braces.TreatmentId);

            // Act
            // Assert
            await Assert.ThrowsAnyAsync<DomainException>(async () =>
            {
                await handler.Handle(command);
            });

            // Check that nothing did not change
            var storedInFakeSpecialty = await repository.GetById(specialty.SpecialtyId);
            Assert.NotNull(storedInFakeSpecialty);

            var updatedTreatment = storedInFakeSpecialty.Treatments.First(t =>
               t.TreatmentId.Equals(braces.TreatmentId));

            Assert.Equal(oldTreatmentName, updatedTreatment.Name);
            Assert.Equal(oldTreatmentBaseCost, updatedTreatment.BaseCost);
            Assert.Equal(oldTreatmentDescription, updatedTreatment.Description);
            Assert.False(repository.SaveWasCalled);
        }

    }
}
