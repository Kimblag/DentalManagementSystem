using DentalSystem.Application.Tests.Builders.Commands.Specialties;
using DentalSystem.Application.Tests.Builders.Domain.Specialties;
using DentalSystem.Application.Tests.Fakes.Repositories.Specialties;
using DentalSystem.Application.UseCases.Specialties.UpdateDescription;
using DentalSystem.Domain.Entities;
using DentalSystem.Domain.Exceptions;
using DentalSystem.Domain.Exceptions.Specialties;

namespace DentalSystem.Application.Tests.UseCases.Specialties.UpdateDescription
{
    public class UpdateSpecialtyDescriptionTest
    {
        // Happy path
        [Fact]
        public async Task Handle_WhenInputDataIsValid_ShouldChangeSpecialtyDescriptionAndPersist()
        {
            // Arrange
            string newDescription = "This is a new description";

            Specialty specialty = SpecialtyBuilder.ActiveWithOneTreatment();
            
            FakeSpecialtyRepository repository = new();
            repository.Add(specialty);
            var handler = new UpdateSpecialtyDescriptionHandler(repository);


            // prepare command
            var command = UpdateSpecialtyDescriptionCommandBuilder.Valid(specialty.SpecialtyId, newDescription);

            // Act
            await handler.Handle(command);

            // Assert
            var storedInFakeSpecialty = await repository.GetById(specialty.SpecialtyId);
            Assert.NotNull(storedInFakeSpecialty);
            Assert.Equal(newDescription, storedInFakeSpecialty.Description?.Value);
            Assert.True(repository.SaveWasCalled);
        }


        [Fact]
        public async Task Handle_WhenDescriptionIsInvalid_ShouldThrowInvalidSpecialtyDescriptionException_AndNotPersist()
        {
            // Arrange
            string invalidDescription = "b2";

            Specialty specialty = SpecialtyBuilder.ActiveWithOneTreatment();
            
            FakeSpecialtyRepository repository = new();
            repository.Add(specialty);
            var handler = new UpdateSpecialtyDescriptionHandler(repository);

            // prepare command
            var command = UpdateSpecialtyDescriptionCommandBuilder.Valid(specialty.SpecialtyId, invalidDescription);

            // Act
            //Assert
            await Assert.ThrowsAnyAsync<DomainException>(async () =>
            {
                await handler.Handle(command);
            });
            Assert.False(repository.SaveWasCalled);
        }


        [Fact]
        public async Task Handle_WhenSpecialtyIsInactive_ShouldThrowInvalidStatusTransitionException_AndNotPersist()
        {
            // Arrange
            string newDescription = "This is a new description";

            Specialty specialty = SpecialtyBuilder.ActiveWithOneTreatment();
            specialty.Deactivate();

            FakeSpecialtyRepository repository = new();
            repository.Add(specialty);

            var command = UpdateSpecialtyDescriptionCommandBuilder.Valid(specialty.SpecialtyId, newDescription);
           
            var handler = new UpdateSpecialtyDescriptionHandler(repository);

            // Act
            // Assert
            await Assert.ThrowsAnyAsync<DomainException>(async () =>
            {
                await handler.Handle(command);
            } );
            Assert.False(repository.SaveWasCalled);
        }


    }
}
