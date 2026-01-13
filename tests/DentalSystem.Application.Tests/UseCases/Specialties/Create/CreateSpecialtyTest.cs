using DentalSystem.Application.Tests.Builders.Commands.Specialties;
using DentalSystem.Application.Tests.Builders.Domain.Specialties;
using DentalSystem.Application.Tests.Fakes.Repositories.Specialties;
using DentalSystem.Application.UseCases.Specialties.Create;
using DentalSystem.Domain.Exceptions;
using DentalSystem.Domain.Exceptions.Specialties;

namespace DentalSystem.Application.Tests.UseCases.Specialties.Create
{
    public class CreateSpecialtyTest
    {
        // Happy path
        [Fact]
        public async Task Handle_WhenInputIsValid_ShouldCreateAndPersistSpecialty()
        {
            // Arrange
            var command = CreateSpecialtyCommandBuilder.Valid();

            var repository = new FakeSpecialtyRepository();
            var handler = new CreateSpecialtyHandler(repository);

            // Act
            await handler.Handle(command);

            // Assert
            // IT must be persisted
            Assert.True(repository.SaveWasCalled);
        }

        // errors
        [Fact]
        public async Task Handle_WhenTreatmentListIsEmpty_ShouldThrowEmptyTreatmentListException()
        {
            // Arrange
            var command = CreateSpecialtyCommandBuilder.InvalidWithTreatments();

            var repository = new FakeSpecialtyRepository();
            var handler = new CreateSpecialtyHandler(repository);

            // Act
            // Assert
            await Assert.ThrowsAnyAsync<DomainException>(async () =>
            {
                await handler.Handle(command);
            });
            Assert.False(repository.SaveWasCalled);
        }

        [Fact]
        public async Task Handle_WhenTreatmentsContainDuplicateNames_ShouldThrowDuplicateTreatmentNameException()
        {
            // Arrange
            var treatmentInput1 = TreatmentInputBuilder.WithName("Braces");
            var treatmentInput2 = TreatmentInputBuilder.WithName(name: "Braces");
            var command = CreateSpecialtyCommandBuilder.InvalidWithTreatments(treatmentInput1, treatmentInput2);

            var repository = new FakeSpecialtyRepository();
            var handler = new CreateSpecialtyHandler(repository);

            // Act
            // Assert
            await Assert.ThrowsAnyAsync<DomainException>(async () =>
            {
                await handler.Handle(command);
            });
            Assert.False(repository.SaveWasCalled);
        }


    }
}
