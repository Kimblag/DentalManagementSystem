using DentalSystem.Domain.Entities;
using DentalSystem.Domain.Exceptions;
using DentalSystem.Domain.Exceptions.Rules.Specialties;
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
            string expectedName = "Orthodontics";
            LifecycleStatus expectedStatus = LifecycleStatus.Active();

            Specialty specialty = SpecialtyBuilder.CreateActiveWithOneTreatment(specialtyName: expectedName);

            Guid originalId = specialty.SpecialtyId;
            string? originalDescription = specialty.Description?.ToString();
            List<Treatment> originalTreatments = [.. specialty.Treatments];

            // Act
            specialty.CorrectName(inputName);

            // Assert
            Assert.Equal(expectedName, specialty.Name.Value);
            Assert.Equal(expectedStatus, specialty.Status);
            specialty.AssertInvariants(
                 originalId,
                 originalTreatments,
                 expectedDescription: originalDescription);
        }


        [Fact]
        public void CorrectName_WhenNameIsIdentical_ShouldNotMutateState()
        {
            // Arrange
            string cleanName = "Orthodontics";
            Specialty specialty = SpecialtyBuilder.CreateActiveWithOneTreatment(cleanName);
            LifecycleStatus expectedStatus = LifecycleStatus.Active();

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
            string expectedName = "Orthodontics";

            Specialty specialty = SpecialtyBuilder.CreateActiveWithOneTreatment(expectedName);
            LifecycleStatus expectedStatus = LifecycleStatus.Active();

            Guid originalId = specialty.SpecialtyId;
            List<Treatment> originalTreatments = [.. specialty.Treatments];

            // Act
            specialty.CorrectName("orthodontics");

            // Assert
            specialty.AssertInvariants(
                 originalId,
                 originalTreatments,
                 expectedName: expectedName);
            Assert.Equal(expectedStatus, specialty.Status);
        }



        // Errors
        [Fact]
        public void CorrectName_WhenSpecialtyIsInactive_ShouldThrowInvalidSpecialtyStateException()
        {
            // Arrange
            Specialty specialty = SpecialtyBuilder.CreateInactive();
            LifecycleStatus expectedStatus = LifecycleStatus.Inactive();

            // take a snapshot of the current state
            Guid originalId = specialty.SpecialtyId;
            List<Treatment> originalTreatments = [.. specialty.Treatments];
            string originalName = specialty.Name.Value;
            string? originalDescription = specialty.Description?.Value;

            // Act
            // Assert
            Assert.Throws<InvalidSpecialtyStateException>(() =>
            {
                specialty.CorrectName("Orthodontics Spec.");
            });

            Assert.Equal(expectedStatus, specialty.Status);
            specialty.AssertInvariants(
                 originalId,
                 originalTreatments,
                 expectedName: originalName,
                 expectedDescription: originalDescription);
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
            LifecycleStatus expectedStatus = LifecycleStatus.Active();

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
