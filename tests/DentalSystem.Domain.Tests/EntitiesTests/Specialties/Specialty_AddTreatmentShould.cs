using DentalSystem.Domain.Entities;
using DentalSystem.Domain.Exceptions.Specialties;
using DentalSystem.Domain.Tests.Builder;

namespace DentalSystem.Domain.Tests.EntitiesTests.Specialties
{
    public class Specialty_AddTreatmentShould
    {
        // Happy path
        [Fact]
        public void AddTreatment_WhenDataIsValid_ShouldAddToCollection()
        {
            // Arrange
            Specialty specialty = SpecialtyBuilder.CreateActiveWithOneTreatment();
            Treatment newTreatment = TreatmentBuilder.CreateValid("Clear Aligners");

            // Act
            // add a valid treatment
            specialty.AddTreatment(newTreatment);

            // Assert
            Assert.Equal(2, specialty.Treatments.Count);
            Assert.Contains(newTreatment, specialty.Treatments);
        }


        [Theory]
        [InlineData("braces")]
        [InlineData("Braces")]
        [InlineData("Braces ")]
        [InlineData(" Braces")]
        [InlineData(" Braces ")]
        public void AddTreatment_WhenNameAlreadyExists_ShouldThrowDuplicateTreatmentNameException(string duplicateTreatmentName)
        {
            // Arrange
            Specialty specialty = SpecialtyBuilder.CreateActiveWithOneTreatment();
            Treatment newTreatment = TreatmentBuilder.CreateValid(duplicateTreatmentName);

            // Act
            // Assert
            Assert.Throws<DuplicateTreatmentNameException>(() =>
            {
                specialty.AddTreatment(newTreatment);
            });
        }


        [Fact]
        public void AddTreatment_WhenSpecialtyIsNotActive_ShouldThrowInvalidSpecialtyStateException()
        {
            // Arrange
            Specialty specialty = SpecialtyBuilder.CreateActiveWithOneTreatment();
            Treatment newTreatment = TreatmentBuilder.CreateValid("Retainers");
            specialty.Deactivate();

            //Act
            //Assert
            Assert.Throws<InvalidSpecialtyStateException>(() =>
            {
                specialty.AddTreatment(newTreatment);
            });
        }


        [Fact]
        public void AddTreatment_WhenTreatmentIsNull_ShouldThrowArgumentNullException()
        {
            // Assert
            Specialty specialty = SpecialtyBuilder.CreateActiveWithOneTreatment();

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                specialty.AddTreatment(null!);
            });
        }
    }
}
