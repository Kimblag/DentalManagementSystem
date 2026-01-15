using DentalSystem.Domain.Entities;
using DentalSystem.Domain.Tests.Builder;
using DentalSystem.Domain.Exceptions;

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
            string specialtyDescription = "Old Description";
            
            string newSpecialtyDescription = "New Description";
            
            Specialty specialty = SpecialtyBuilder.CreateActiveWithOneTreatment(specialtyName, specialtyDescription);

            // Act
            specialty.UpdateDescription(newSpecialtyDescription);

            // Assert
            // Check that the name is the same
            Assert.Equal(specialtyName, specialty.Name.Value);

            // Check the description has changed.
            Assert.Equal(newSpecialtyDescription, specialty.Description?.Value);
        }


        [Fact]
        public void UpdateDescription_WhenNullOrEmpty_ShouldClearProperty()
        {
            // Arrange
            string specialtyName = "Endodontics";
            string specialtyDescription = "Old Description";

            Specialty specialty = SpecialtyBuilder.CreateActiveWithOneTreatment(specialtyName, specialtyDescription);

            // Act
            specialty.UpdateDescription(null);

            // Assert
            Assert.Equal(specialtyName, specialty.Name.Value);
            Assert.Null(specialty.Description?.Value);
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
            Assert.ThrowsAny<DomainException>(() =>
            {
                specialty.UpdateDescription("New Description");
            });
        }


        [Fact]
        public void UpdateDescription_WhenDescriptionDoesNotMatchPattern_ShouldThrowInvalidSpecialtyDescriptionException()
        {
            // Arrange
            Specialty specialty = SpecialtyBuilder.CreateActiveWithOneTreatment();

            // Act
            // Assert
            Assert.ThrowsAny<DomainException>(() =>
            {
                specialty.UpdateDescription("!!");
            });
        }

    }
}
