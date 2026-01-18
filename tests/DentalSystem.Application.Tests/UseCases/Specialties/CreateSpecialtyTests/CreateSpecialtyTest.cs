using DentalSystem.Application.Tests.Fakes.Persistence;
using DentalSystem.Application.Tests.Fakes.Repositories.Specialties;
using DentalSystem.Application.UseCases.Specialties.CreateSpecialty;
using DentalSystem.Domain.Exceptions.Specialties;

namespace DentalSystem.Application.Tests.UseCases.Specialties.CreateSpecialtyTests
{
    public sealed class CreateSpecialtyTest
    {
        // Happy path
        [Fact]
        public async Task Handle_WhenInputIsValid_ShouldCreateAndPersistSpecialty()
        {
            // Arrange
            FakeUnitOfWork unitOfWork = new();
            FakeSpecialtyRepository repository = new();

            CreateSpecialtyHandler handler = new(repository, unitOfWork);

            List<TreatmentInput> treatments =
            [
                new("Clear Aligners", 25.0m, "Removable transparent aligners."),
                new("Retainers", 13.0m, "Devices used to maintain teeth position.")
            ];
            CreateSpecialtyCommand command = new("Endodontics", treatments, "A new specialty");

            // Act
            await handler.Handle(command, CancellationToken.None);

           
        }


        // errors
        [Fact]
        public async Task Handle_WhenTreatmentListIsEmpty_ShouldThrowEmptyTreatmentListException()
        {
            // Arrange
            FakeUnitOfWork unitOfWork = new();
            FakeSpecialtyRepository repository = new();

            CreateSpecialtyHandler handler = new(repository, unitOfWork);

            List<TreatmentInput> treatments = [];
            CreateSpecialtyCommand command = new("Endodontics", treatments, "A new specialty");

            // Act
            // Assert
            await Assert.ThrowsAsync<EmptyTreatmentListException>(async () =>
            {
                await handler.Handle(command, CancellationToken.None);
            });
        }


        [Fact]
        public async Task Handle_WhenTreatmentsContainDuplicateNames_ShouldThrowDuplicateTreatmentNameException()
        {
            // Arrange
            FakeUnitOfWork unitOfWork = new();
            FakeSpecialtyRepository repository = new();

            CreateSpecialtyHandler handler = new(repository, unitOfWork);

            List<TreatmentInput> treatments =
            [
                new("Retainers", 25.0m, "Removable transparent aligners."),
                new("Retainers", 13.0m, "Devices used to maintain teeth position.")
            ];
            CreateSpecialtyCommand command = new("Orthodontics", treatments, "A new specialty");

            // Act
            // Assert
            await Assert.ThrowsAsync<DuplicateTreatmentNameException>(async () =>
            {
                await handler.Handle(command, CancellationToken.None);
            });
        }


    }
}
