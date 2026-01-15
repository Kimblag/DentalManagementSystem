using DentalSystem.Application.Tests.Builders.Domain.Specialties;
using DentalSystem.Application.Tests.Fakes.Persistence;
using DentalSystem.Application.Tests.Fakes.Repositories.Specialties;
using DentalSystem.Application.UseCases.Specialties.EditSpecialty.CorrectSpecialtyName;
using DentalSystem.Domain.Entities;
using DentalSystem.Domain.Exceptions.Specialties;
using DentalSystem.Domain.Exceptions.ValueObjects;

namespace DentalSystem.Application.Tests.UseCases.Specialties.EditSpecialtyTests.CorrectSpecialtyNameTests
{
    public sealed class CorrectSpecialtyNameTest
    {
        // Happy path
        [Fact]
        public async Task Handle_WhenInputNameIsValid_ShouldChangeSpecialtyName_AndPersist()
        {
            // Arrange
            Specialty specialty = SpecialtyBuilder.CreateActiveWithOneTreatment("Enodontics");

            CorrectSpecialtyNameCommand command = new(specialty.SpecialtyId, "Endodonthics");

            FakeUnitOfWork unitOfWork = new();
            FakeSpecialtyRepository repository = new(unitOfWork);
            repository.Add(specialty);

            CorrectSpecialtyNameHandler handler = new(repository, unitOfWork);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            var stored = await repository.GetById(specialty.SpecialtyId, CancellationToken.None);
            Assert.Equal("Endodonthics", stored!.Name.Value);
            Assert.True(unitOfWork.WasCommitted);
        }

        // Errors
        [Fact]
        // correct name with identical value
        public async Task Handle_WhenNewNameIsIdentical_ShouldNotChangeSpecialtyName_AndNotPersist()
        {
            // Arrange
            Specialty specialty = SpecialtyBuilder.CreateActiveWithOneTreatment("Endodonthics");

            CorrectSpecialtyNameCommand command = new(specialty.SpecialtyId, "Endodonthics");
            
            FakeUnitOfWork unitOfWork = new();
            FakeSpecialtyRepository repository = new(unitOfWork);
            repository.Add(specialty);

            CorrectSpecialtyNameHandler handler = new(repository, unitOfWork);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            var stored = await repository.GetById(specialty.SpecialtyId, CancellationToken.None);
            Assert.Equal("Endodonthics", stored!.Name.Value);
            Assert.False(unitOfWork.WasCommitted);
        }


        // Attempt to correct name in an inactive specialty
        [Fact]
        public async Task Handle_WhenSpecialtyIsInactive_ShouldThrowInvalidSpecialtyStateException()
        {
            // Arrange
            Specialty specialty = SpecialtyBuilder.CreateInactive("Enodontics");

            CorrectSpecialtyNameCommand command = new(specialty.SpecialtyId, "Endodonthics");

            FakeUnitOfWork unitOfWork = new();
            FakeSpecialtyRepository repository = new(unitOfWork);
            repository.Add(specialty);

            CorrectSpecialtyNameHandler handler = new(repository, unitOfWork);


            // Act
            // Assert
            await Assert.ThrowsAsync<InvalidSpecialtyStateException>(async () => 
            {
                await handler.Handle(command, CancellationToken.None);
            });
            Assert.False(unitOfWork.WasCommitted);
        }


        // Attempt to correct name with invalid inputs
        [Theory]
        [InlineData(" ")]
        [InlineData("")]
        [InlineData("!!")]
        [InlineData("a1")]
        public async Task Handle_WhenNameIsInvalid_ShouldThrowInvalidNameException(string invalidName)
        {
            // Arrange
            Specialty specialty = SpecialtyBuilder.CreateActiveWithOneTreatment("Enodontics");

            CorrectSpecialtyNameCommand command = new(specialty.SpecialtyId, invalidName);

            FakeUnitOfWork unitOfWork = new();
            FakeSpecialtyRepository repository = new(unitOfWork);
            repository.Add(specialty);

            CorrectSpecialtyNameHandler handler = new(repository, unitOfWork);

            // Act
            //Assert
            await Assert.ThrowsAsync<InvalidNameException>(async () =>
            {
                await handler.Handle(command, CancellationToken.None);
            });
            Assert.False(unitOfWork.WasCommitted);
        }
    }
}
