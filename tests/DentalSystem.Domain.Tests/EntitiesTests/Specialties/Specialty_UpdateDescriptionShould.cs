using DentalSystem.Domain.Entities;
using DentalSystem.Domain.Tests.Builder;
using DentalSystem.Domain.Exceptions.Specialties;

namespace DentalSystem.Domain.Tests.EntitiesTests.Specialties
{
    public class Specialty_UpdateDescriptionShould
    {
        // happy path
        [Fact]
        public void UpdateDescription_WhenValidString_ShouldUpdateProperty()
        {
            // Arrange
            string specialtyName = "Endodontics";
            string newSpecialtyDescription = "New Description";
            Treatment treatment = TreatmentBuilder.CreateValid();
            Specialty specialty = SpecialtyBuilder.CreateActiveWithTreatments([treatment], specialtyName, "Old description");

            // Act
            specialty.UpdateDescription(newSpecialtyDescription);

            // Assert
            Assert.Equal(specialtyName, specialty.Name);
            Assert.Equal(newSpecialtyDescription, specialty.Description);
        }

        [Fact]
        public void UpdateDescription_WhenNullOrEmpty_ShouldClearProperty()
        {
            // Arrange
            string specialtyName = "Endodontics";
            Treatment treatment = TreatmentBuilder.CreateValid();
            Specialty specialty = SpecialtyBuilder.CreateActiveWithTreatments([treatment], specialtyName, "Old description");

            // Act
            specialty.UpdateDescription(string.Empty);

            // Assert
            Assert.Equal(specialtyName, specialty.Name);
            Assert.Equal(string.Empty, specialty.Description);
        }


        // errors
        [Fact]
        public void UpdateDescription_WhenSpecialtyIsNotActive_ShouldThrowInvalidSpecialtyStateException()
        {
            // Arrange
            Specialty specialty = SpecialtyBuilder.CreateActiveWithOneTreatment();
            specialty.Deactivate();

            // Act
            // Assert
            Assert.Throws<InvalidSpecialtyStateException>(() =>
            {
                specialty.UpdateDescription("New Description");
            });
        }


        [Fact]
        public void UpdateDescription_WhenDescriptionDoesNotMatchPattern_ShouldThrowInvalidSpecialtyDescriptionException()
        {
            // Arrange
            Specialty specialty = SpecialtyBuilder.CreateActiveWithOneTreatment();
            string invalidDescription = "1s";

            // Act
            // Assert
            Assert.Throws<InvalidSpecialtyDescriptionException>(() =>
            {
                specialty.UpdateDescription(invalidDescription);
            });
        }

    }
}
