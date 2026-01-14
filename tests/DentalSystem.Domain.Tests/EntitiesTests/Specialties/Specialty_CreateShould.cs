using DentalSystem.Domain.Entities;
using DentalSystem.Domain.Exceptions;
using DentalSystem.Domain.Tests.Builder;
using DentalSystem.Domain.ValueObjects;

namespace DentalSystem.Domain.Tests.EntitiesTests.Specialties
{
    public class Specialty_CreateShould
    {
        // Happy path
        [Fact]
        public void Create_WhenDataIsValid_ShouldCreateActiveSpecialtyWithTreatments()
        {
            // Follow A-A-A: Arrange - Act - Assert

            // Arrange
            string specialtyName = "Orthodontics";
            string specialtyDescription = "Focuses on correcting teeth and jaw alignment issues.";

            Treatment braces = TreatmentBuilder.CreateValid();

            // Act
            Specialty specialty = SpecialtyBuilder.CreateActiveWithTreatments(
                [braces],
                specialtyName,
                specialtyDescription);

            // Assert
            Assert.Equal(specialtyName, specialty.Name);
            Assert.Equal(specialtyDescription, specialty.Description?.Value);
            Assert.True(specialty.Status.IsActive);

            Assert.Single(specialty.Treatments);
            Assert.Contains(braces, specialty.Treatments);
        }



        [Fact]
        public void Create_WhenTreatmentListIsEmpty_ShouldThrowDomainException()
        {
            // Arrange
            Name specialtyName = new("Orthodontics");
            Description specialtyDescription = new("Focuses on correcting teeth and jaw alignment issues.");
            List<Treatment> invalidTreatments = [];

            // Act and Assert
            Assert.ThrowsAny<DomainException>(() =>
            {
                new Specialty(specialtyName, invalidTreatments, specialtyDescription);
            });
        }
        

        [Fact]
        public void Create_WhenTreatmentNamesAreDuplicated_ShouldThrowDomainException()
        {
            // Arrange
            string duplicateName = "Braces";
            Treatment treatment1 = TreatmentBuilder.CreateValid(duplicateName);
            Treatment treatment2 = TreatmentBuilder.CreateValid(duplicateName);

            Name specialtyName = new("Orthodontics");
            Description specialtyDescription = new("Focuses on correcting teeth and jaw alignment issues.");

            // Act and Assert
            Assert.ThrowsAny<DomainException>(() =>
            {
                new Specialty(specialtyName, [treatment1, treatment2], specialtyDescription);
            });
        }


    }
}
