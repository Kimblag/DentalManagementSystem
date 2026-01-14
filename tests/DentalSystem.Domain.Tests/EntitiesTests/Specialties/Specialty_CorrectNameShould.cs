using DentalSystem.Domain.Entities;
using DentalSystem.Domain.Exceptions;
using DentalSystem.Domain.Exceptions.Specialties;
using DentalSystem.Domain.Tests.Builder;
using DentalSystem.Domain.Tests.Helpers;
using DentalSystem.Domain.ValueObjects;

namespace DentalSystem.Domain.Tests.EntitiesTests.Specialties
{
    public class Specialty_CorrectNameShould
    {
        // happy path
        [Theory]
        [InlineData("Orthodontics")]
        [InlineData(" Orthodontics")]
        [InlineData(" Orthodontics ")]
        public void CorrectName_WhenValidName_ShouldUpdateName_AndPreserveInvariants(string inputName)
        {
            // Arrange
            Specialty specialty = SpecialtyBuilder.CreateActiveWithOneTreatment("Orthdontics");
            var originalId = specialty.SpecialtyId;
            List<Treatment> originalTreatments = [.. specialty.Treatments];
            string? originalDescription = specialty.Description?.ToString();
            LifecycleStatus originalStatus = specialty.Status;

            string expectedName = "Orthodontics";

            // Act
            specialty.CorrectName(inputName);

            // Assert
            Assert.Equal(expectedName, specialty.Name);
            specialty.AssertInvariants(
                 originalId,
                 originalStatus,
                 originalTreatments,
                 expectedDescription: originalDescription);
        }


        [Fact]
        public void CorrectName_WhenNameIsIdentical_ShouldReturnWithoutAnyChanges_ToAnyProperty()
        {
            // Arrange
            string cleanName = "Orthodontics";
            Specialty specialty = SpecialtyBuilder.CreateActiveWithOneTreatment(cleanName);

            // snapshot
            Guid originalId = specialty.SpecialtyId;
            string? originalDescription = specialty.Description?.ToString();
            List<Treatment> originalTreatments = [.. specialty.Treatments];
            LifecycleStatus originalStatus = specialty.Status;

            // Act
            specialty.CorrectName(cleanName);

            // Assert
            specialty.AssertInvariants(
                  originalId,
                  originalStatus,
                  originalTreatments,
                  expectedName: cleanName,
                  expectedDescription: originalDescription);
        }


        [Fact]
        public void CorrectName_WhenOnlyCaseDiffers_ShouldNotMutate()
        {
            // Arrange
            Specialty specialty = SpecialtyBuilder.CreateActiveWithOneTreatment("Orthodontics");
            List<Treatment> originalTreatments = [.. specialty.Treatments];
            var originalId = specialty.SpecialtyId;
            LifecycleStatus originalStatus = specialty.Status;

            // Act
            specialty.CorrectName("orthodontics");

            // Assert
            specialty.AssertInvariants(
                 originalId,
                 originalStatus,
                 originalTreatments,
                 expectedName: "Orthodontics");
        }



        // Errors
        [Fact]
        public void CorrectName_WhenSpecialtyIsInactive_ShouldThrowException_AndPreserveState()
        {
            // Arrange
            Specialty specialty = SpecialtyBuilder.CreateActiveWithOneTreatment("Orthdontics");
            specialty.Deactivate();

            // take a snapshot of the current state
            Guid originalId = specialty.SpecialtyId;
            List<Treatment> originalTreatments = [.. specialty.Treatments];
            string originalName = specialty.Name.ToString();
            string? originalDescription = specialty.Description?.ToString();
            LifecycleStatus originalStatus = specialty.Status;

            // Act
            // Assert
            Assert.Throws<InvalidSpecialtyStateException>(() =>
            {
                specialty.CorrectName("Orthodontics");
            });

            specialty.AssertInvariants(
                 originalId,
                 originalStatus,
                 originalTreatments,
                 expectedName: originalName,
                 expectedDescription: originalDescription);
        }


        [Theory]
        [InlineData("")]
        [InlineData("or")]
        [InlineData(null)]
        public void CorrectName_WhenNameIsInvalid_ShouldThrowException_AndNotMutateName(string? invalidName)
        {
            // Arrange
            string originalName = "Orthdontics";
            Specialty specialty = SpecialtyBuilder.CreateActiveWithOneTreatment(originalName);

            Guid originalId = specialty.SpecialtyId;
            List<Treatment> originalTreatments = [.. specialty.Treatments];
            string? originalDescription = specialty.Description?.ToString();
            LifecycleStatus originalStatus = specialty.Status;

            // Act
            // Assert
            Assert.ThrowsAny<DomainException>(() =>
            {
                specialty.CorrectName(invalidName!);
            });

            specialty.AssertInvariants(
                originalId,
                originalStatus,
                originalTreatments,
                expectedName: originalName,
                expectedDescription: originalDescription);
        }

    }
}
