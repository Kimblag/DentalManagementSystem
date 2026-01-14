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
            var expectedStatus = LifecycleStatus.Active();

            var originalId = specialty.SpecialtyId;
            List<Treatment> originalTreatments = [.. specialty.Treatments];
            string? originalDescription = specialty.Description?.ToString();

            string expectedName = "Orthodontics";

            // Act
            specialty.CorrectName(inputName);

            // Assert
            Assert.Equal(expectedName, specialty.Name);
            specialty.AssertInvariants(
                 originalId,
                 originalTreatments,
                 expectedDescription: originalDescription);
            Assert.Equal(expectedStatus, specialty.Status);
        }


        [Fact]
        public void CorrectName_WhenNameIsIdentical_ShouldNotMutateState()
        {
            // Arrange
            string cleanName = "Orthodontics";
            Specialty specialty = SpecialtyBuilder.CreateActiveWithOneTreatment(cleanName);
            var expectedStatus = LifecycleStatus.Active();

            // snapshot
            Guid originalId = specialty.SpecialtyId;
            string? originalDescription = specialty.Description?.ToString();
            List<Treatment> originalTreatments = [.. specialty.Treatments];

            // Act
            specialty.CorrectName(cleanName);

            // Assert
            specialty.AssertInvariants(
                  originalId,
                  originalTreatments,
                  expectedName: cleanName,
                  expectedDescription: originalDescription);
            Assert.Equal(expectedStatus, specialty.Status);
        }


        [Fact]
        public void CorrectName_WhenOnlyCaseDiffers_ShouldNotMutate()
        {
            // Arrange
            Specialty specialty = SpecialtyBuilder.CreateActiveWithOneTreatment("Orthodontics");
            var expectedStatus = LifecycleStatus.Active();

            List<Treatment> originalTreatments = [.. specialty.Treatments];
            var originalId = specialty.SpecialtyId;

            // Act
            specialty.CorrectName("orthodontics");

            // Assert
            specialty.AssertInvariants(
                 originalId,
                 originalTreatments,
                 expectedName: "Orthodontics");
            Assert.Equal(expectedStatus, specialty.Status);
        }



        // Errors
        [Fact]
        public void CorrectName_WhenSpecialtyIsInactive_ShouldThrowInvalidSpecialtyStateException()
        {
            // Arrange
            Specialty specialty = SpecialtyBuilder.CreateActiveWithOneTreatment("Orthdontics");
            specialty.Deactivate();
            var expectedStatus = LifecycleStatus.Inactive();

            // take a snapshot of the current state
            Guid originalId = specialty.SpecialtyId;
            List<Treatment> originalTreatments = [.. specialty.Treatments];
            string originalName = specialty.Name.ToString();
            string? originalDescription = specialty.Description?.ToString();

            // Act
            // Assert
            Assert.Throws<InvalidSpecialtyStateException>(() =>
            {
                specialty.CorrectName("Orthodontics");
            });

            specialty.AssertInvariants(
                 originalId,
                 originalTreatments,
                 expectedName: originalName,
                 expectedDescription: originalDescription);
            Assert.Equal(expectedStatus, specialty.Status);
        }


        [Theory]
        [InlineData("")]
        [InlineData("or")]
        [InlineData(null)]
        public void CorrectName_WhenNameIsInvalid_ShouldThrowDomainException(string? invalidName)
        {
            // Arrange
            string originalName = "Orthdontics";
            Specialty specialty = SpecialtyBuilder.CreateActiveWithOneTreatment(originalName);
            var expectedStatus = LifecycleStatus.Active();

            Guid originalId = specialty.SpecialtyId;
            List<Treatment> originalTreatments = [.. specialty.Treatments];
            string? originalDescription = specialty.Description?.ToString();

            // Act
            // Assert
            Assert.ThrowsAny<DomainException>(() =>
            {
                specialty.CorrectName(invalidName!);
            });

            specialty.AssertInvariants(
                originalId,
                originalTreatments,
                expectedName: originalName,
                expectedDescription: originalDescription);
            Assert.Equal(expectedStatus, specialty.Status);
        }

    }
}
