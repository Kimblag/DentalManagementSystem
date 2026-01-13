using DentalSystem.Application.Tests.Builders.Commands.Specialties;
using DentalSystem.Application.Tests.Builders.Domain.Specialties;
using DentalSystem.Application.Tests.Fakes.Repositories.Specialties;
using DentalSystem.Application.UseCases.Specialties.CorrectName;
using DentalSystem.Domain.Entities;
using DentalSystem.Domain.Exceptions;

namespace DentalSystem.Application.Tests.UseCases.Specialties.CorrectName
{
    public class CorrectSpecialtyNameTest
    {
        // Happy Path
        [Fact]
        public async Task Handle_WhenInputNameIsValid_ShouldChangeSpecialtyName_AndPersist()
        {
            // Arrange
            string newName = "Orthodontics";
            Specialty specialty = SpecialtyBuilder.ActiveWithOneTreatmentAndName("Ortohdontics");

            FakeSpecialtyRepository repository = new();
            repository.Add(specialty);

            var command = CorrectSpecialtyNameCommandBuilder.WithName(specialty.SpecialtyId, newName);
            var handler = new CorrectSpecialtyNameHandler(repository);

            // Act
            await handler.Handle(command);


            // Assert
            var storedInFakeSpecialty = await repository.GetById(specialty.SpecialtyId);
            Assert.NotNull(storedInFakeSpecialty);
            Assert.Equal(newName, storedInFakeSpecialty.Name);
            Assert.True(repository.SaveWasCalled);
        }


        // errors

        // Correct name with identical value
        [Fact]
        public async Task Handle_WhenNewNameIsIdentical_ShouldNotChangeSpecialtyName_AndNotPersist()
        {
            // Arrange
            string newName = "Orthodontics";
            Specialty specialty = SpecialtyBuilder.ActiveWithOneTreatment();

            FakeSpecialtyRepository repository = new();
            repository.Add(specialty);

            var command = CorrectSpecialtyNameCommandBuilder.WithName(specialty.SpecialtyId, newName);
            var handler = new CorrectSpecialtyNameHandler(repository);

            // Act
            await handler.Handle(command);

            // Assert
            var storedInFakeSpecialty = await repository.GetById(specialty.SpecialtyId);
            Assert.NotNull(storedInFakeSpecialty);
            // check it does not change
            Assert.Equal(newName, storedInFakeSpecialty.Name);
            Assert.True(repository.SaveWasCalled);
        }

        // Correct name in an inactive specialty
        [Fact]
        public async Task Handle_WhenSpecialtyIsInactive_ShouldThrowInvalidSpecialtyStateException()
        {
            // Arrange
            string specialtyOldName = "Orhoodontics";
            string newName = "Orthodontics";
            
            Specialty specialty = SpecialtyBuilder.ActiveWithOneTreatmentAndName(specialtyOldName);
            specialty.Deactivate();

            FakeSpecialtyRepository repository = new();
            repository.Add(specialty);

            var command = CorrectSpecialtyNameCommandBuilder.WithName(specialty.SpecialtyId, newName);
            var handler = new CorrectSpecialtyNameHandler(repository);

            // Act
            //Assert
            await Assert.ThrowsAnyAsync<DomainException>(async () =>
            {
                await handler.Handle(command);
            });
            var storedInFakeSpecialty = await repository.GetById(specialty.SpecialtyId);
            Assert.NotNull(storedInFakeSpecialty);
            // check it does not change
            Assert.Equal(specialtyOldName, storedInFakeSpecialty.Name);
            Assert.False(repository.SaveWasCalled);
        }


        // Correct name with invalid input: blank or whitespaces
        [Theory]
        [InlineData(" ")]
        [InlineData("")]
        public async Task Handle_WhenNameIsBlankOrWhiteSpace_ShouldThrowInvalidSpecialtyNameException(string invalidName)
        {
            // Arrange
            string oldName = "Orhoodontics";
            Specialty specialty = SpecialtyBuilder.ActiveWithOneTreatmentAndName(oldName);

            FakeSpecialtyRepository repository = new();
            repository.Add(specialty);

            var command = CorrectSpecialtyNameCommandBuilder.WithName(specialty.SpecialtyId, invalidName);
            var handler = new CorrectSpecialtyNameHandler(repository);

            // Act
            //Assert
            await Assert.ThrowsAnyAsync<DomainException>(async () =>
            {
                await handler.Handle(command);
            });
            var storedInFakeSpecialty = await repository.GetById(specialty.SpecialtyId);
            Assert.NotNull(storedInFakeSpecialty);
            // check it does not change
            Assert.Equal(oldName, storedInFakeSpecialty.Name);
            Assert.False(repository.SaveWasCalled);
        }


        // Correct name with invalid input: invalid format
        [Fact]
        public async Task Handle_WhenNameHasInvalidFormat_ShouldThrowInvalidSpecialtyNameException()
        {
            // Arrange
            string oldName = "Orhoodontics";
            string invalidName = "!!";

            Specialty specialty = SpecialtyBuilder.ActiveWithOneTreatmentAndName(oldName);

            FakeSpecialtyRepository repository = new();
            repository.Add(specialty);

            var command = CorrectSpecialtyNameCommandBuilder.WithName(specialty.SpecialtyId, invalidName);
            var handler = new CorrectSpecialtyNameHandler(repository);

            // Act
            //Assert
            await Assert.ThrowsAnyAsync<DomainException>(async () =>
            {
                await handler.Handle(command);
            });
            var storedInFakeSpecialty = await repository.GetById(specialty.SpecialtyId);
            Assert.NotNull(storedInFakeSpecialty);
            // check it does not change
            Assert.Equal(oldName, storedInFakeSpecialty.Name);
            Assert.False(repository.SaveWasCalled);
        }
    }
}
