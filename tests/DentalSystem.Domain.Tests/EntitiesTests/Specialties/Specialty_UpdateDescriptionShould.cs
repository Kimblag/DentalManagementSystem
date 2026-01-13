using DentalSystem.Domain.Entities;
using DentalSystem.Domain.ValueObjects;
using DentalSystem.Domain.Tests.Builder;
using DentalSystem.Domain.Exceptions.Specialties;
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
            string newSpecialtyDescription = "New Description";
            Treatment treatment = TreatmentBuilder.CreateValid();
            Specialty specialty = SpecialtyBuilder.CreateActiveWithTreatments([treatment], specialtyName, "Old description");

            // Act
            specialty.UpdateDescription(new Description(newSpecialtyDescription));

            // Assert
            Assert.Equal(specialtyName, specialty.Name);
            Assert.Equal(newSpecialtyDescription, specialty.Description?.Value);
        }

        [Fact]
        public void UpdateDescription_WhenNullOrEmpty_ShouldClearProperty()
        {
            // Arrange
            string specialtyName = "Endodontics";
            Treatment treatment = TreatmentBuilder.CreateValid();
            Specialty specialty = SpecialtyBuilder.CreateActiveWithTreatments([treatment], specialtyName, "Old description");

            // Act
            specialty.UpdateDescription(null);

            // Assert
            Assert.Equal(specialtyName, specialty.Name);
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
                specialty.UpdateDescription(new Description("New Description"));
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
                specialty.UpdateDescription(new Description("!!"));
            });
        }

    }
}
