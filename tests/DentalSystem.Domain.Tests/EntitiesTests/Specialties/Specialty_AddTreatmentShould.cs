using DentalSystem.Domain.Entities;
using DentalSystem.Domain.Exceptions.Rules.Specialties;
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

            // Act
            // add a valid treatment
            specialty.AddTreatment("Retainers", 20, "A new treatment");

            // Assert
            Assert.Equal(2, specialty.Treatments.Count);
            Assert.Contains(specialty.Treatments, t => t.Name.Value == "Retainers");
        }


        [Theory]
        [InlineData("braces")]
        [InlineData("Braces")]
        [InlineData("Braces ")]
        [InlineData(" Braces")]
        [InlineData(" Braces ")]
        public void AddTreatment_WhenTreatmentNameIsEquivalent_ShouldThrowDuplicateTreatmentNameException(string duplicateTreatmentName)
        {
            // Arrange
            Specialty specialty = SpecialtyBuilder.CreateActiveWithOneTreatment();
           

            // Act
            // Assert
            Assert.Throws<DuplicateTreatmentNameException>(() =>
            {
                specialty.AddTreatment(duplicateTreatmentName, 20, "A new treatment");
            });
        }


        [Fact]
        public void AddTreatment_WhenSpecialtyIsInactive_ShouldThrowInvalidSpecialtyStateException()
        {
            // Arrange
            Specialty inactiveSpecialty = SpecialtyBuilder.CreateInactive();
           
            //Act
            //Assert
            Assert.Throws<InvalidSpecialtyStateException>(() =>
            {
                inactiveSpecialty.AddTreatment("Retainers", 20, "A new treatment");
            });
        }

    }
}
