using DentalSystem.Domain.Entities;
using DentalSystem.Domain.Enums;
using DentalSystem.Domain.Exceptions;
using DentalSystem.Domain.Exceptions.Specialties;
using DentalSystem.Domain.Tests.Builder;
using DentalSystem.Domain.Tests.Helpers;

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
            var originalTreatments = specialty.Treatments.ToList();
            var originalDescription = specialty.Description;

            string expectedName = "Orthodontics";

            // Act
            specialty.CorrectName(inputName);

            // Assert
            Assert.Equal(expectedName, specialty.Name);
            specialty.AssertInvariants(
                 originalId,
                 EntityStatus.Active,
                 originalTreatments,
                 expectedDescription: originalDescription);
        }


        [Fact]
        public void CorrectName_WhenNameIsIdentical_ShouldReturnWithoutAnyChanges_ToAnyProperty()
        {
            // Arrange
            string cleanName = "Orthodontics";
            var specialty = SpecialtyBuilder.CreateActiveWithOneTreatment(cleanName);


            var originalId = specialty.SpecialtyId;
            var originalDescription = specialty.Description;
            var originalTreatments = specialty.Treatments.ToList();

            // Act
            specialty.CorrectName(cleanName);

            // Assert
            specialty.AssertInvariants(
                  originalId,
                  EntityStatus.Active,
                  originalTreatments,
                  expectedName: cleanName,
                  expectedDescription: originalDescription);
        }


        [Fact]
        public void CorrectName_WhenOnlyCaseDiffers_ShouldNotMutate()
        {
            // Arrange
            var specialty = SpecialtyBuilder.CreateActiveWithOneTreatment("Orthodontics");
            var originalTreatments = specialty.Treatments.ToList();
            var originalId = specialty.SpecialtyId;

            // Act
            specialty.CorrectName("orthodontics");

            // Assert
            specialty.AssertInvariants(
                 originalId,
                 EntityStatus.Active,
                 originalTreatments,
                 expectedName: "Orthodontics");
        }



        // Errors
        [Fact]
        public void CorrectName_WhenSpecialtyIsInactive_ShouldThrowException_AndPreserveState()
        {
            // Arrange
            var specialty = SpecialtyBuilder.CreateActiveWithOneTreatment("Orthdontics");
            specialty.Deactivate();

            // take a snapshot of the current state
            var originalId = specialty.SpecialtyId;
            var originalTreatments = specialty.Treatments.ToList();
            var originalName = specialty.Name;
            var originalDescription = specialty.Description;

            // Act
            // Assert
            Assert.Throws<InvalidSpecialtyStateException>(() =>
            {
                specialty.CorrectName("Orthodontics");
            });

            specialty.AssertInvariants(
                 originalId,
                 EntityStatus.Inactive,
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
            var specialty = SpecialtyBuilder.CreateActiveWithOneTreatment(originalName);

            var originalId = specialty.SpecialtyId;
            var originalTreatments = specialty.Treatments.ToList();
            var originalDescription = specialty.Description;

            // Act
            // Assert
            Assert.ThrowsAny<DomainException>(() =>
            {
                specialty.CorrectName(invalidName!);
            });

            specialty.AssertInvariants(
                originalId,
                EntityStatus.Active,
                originalTreatments,
                expectedName: originalName,
                expectedDescription: originalDescription);
        }

    }
}
