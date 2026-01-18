using DentalSystem.Application.Tests.Builders.Domain.Specialties;
using DentalSystem.Application.Tests.Fakes.Persistence;
using DentalSystem.Application.Tests.Fakes.Repositories.Specialties;
using DentalSystem.Application.UseCases.Specialties.EditSpecialty.UpdateSpecialtyDescription;
using DentalSystem.Domain.Entities;
using DentalSystem.Domain.Exceptions.Specialties;
using DentalSystem.Domain.Exceptions.ValueObjects;

namespace DentalSystem.Application.Tests.UseCases.Specialties.EditSpecialtyTests.UpdateSpecialtyDescriptionTests
{
    public sealed class UpdateSpecialtyDescription
    {
        // happy path
        [Theory]
        [InlineData("Updated description")]
        [InlineData(null)]
        public async Task Handle_WhenInputDataIsValid_ShouldChangeSpecialtyDescriptionAndPersist(string? updatedDescription)
        {
            // Arrange
            Specialty specialty = SpecialtyBuilder.CreateActiveWithOneTreatment();

            FakeUnitOfWork unitOfWork = new();
            FakeSpecialtyRepository repository = new();

            repository.Add(specialty);

            UpdateSpecialtyDescriptionCommand command = new(specialty.SpecialtyId, updatedDescription);
            UpdateSpecialtyDescriptionHandler handler = new(repository, unitOfWork);


            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            var stored = await repository.GetByIdAsync(specialty.SpecialtyId, CancellationToken.None);
            Assert.Equal(updatedDescription, stored!.Description?.Value);
        }



        // Errors
        [Fact]
        public async Task Handle_WhenDescriptionIsInvalid_ShouldThrowInvalidDescriptionException_AndNotPersist()
        {
            // Arrange
            Specialty specialty = SpecialtyBuilder.CreateActiveWithOneTreatment();

            FakeUnitOfWork unitOfWork = new();
            FakeSpecialtyRepository repository = new();

            repository.Add(specialty);

            UpdateSpecialtyDescriptionCommand command = new(specialty.SpecialtyId, "!!");
            UpdateSpecialtyDescriptionHandler handler = new(repository, unitOfWork);


            // Act
            // Assert
            await Assert.ThrowsAsync<InvalidDescriptionException>(async () =>
            {
                await handler.Handle(command, CancellationToken.None);
            });
        }


        [Fact]
        public async Task Handle_WhenSpecialtyIsInactive_ShouldThrowInvalidSpecialtyStateException_AndNotPersist()
        {
            // Arrange
            Specialty specialty = SpecialtyBuilder.CreateInactive();

            FakeUnitOfWork unitOfWork = new();
            FakeSpecialtyRepository repository = new();

            repository.Add(specialty);

            UpdateSpecialtyDescriptionCommand command = new(specialty.SpecialtyId, "Updated description");
            UpdateSpecialtyDescriptionHandler handler = new(repository, unitOfWork);


            // Act
            // Assert
            await Assert.ThrowsAsync<InvalidSpecialtyStateException>(async () =>
            {
                await handler.Handle(command, CancellationToken.None);
            });
        }

    }
}
