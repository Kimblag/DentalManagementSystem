using DentalSystem.Domain.Entities;
using DentalSystem.Domain.Enums;
using DentalSystem.Domain.Exceptions;
using DentalSystem.Domain.Tests.Builder;
using DentalSystem.Domain.ValueObjects;

namespace DentalSystem.Domain.Tests.EntitiesTests.Specialties
{
    public class Specialty_CreateShould
    {
        // Happy path
        [Fact]
        public void Create_WhenDataIsValid_ShouldCreateSpecialtySuccessfully()
        {
            // Follow A-A-A: Arrange - Act - Assert

            // Arrange
            string specialtyName = "Orthodontics";
            string specialtyDescription = "Focuses on correcting teeth and jaw alignment issues.";
            string treatmentName = "Braces";
            decimal treatmentBaseCost = 10.0m;
            string treatmentDescription = "Metal or ceramic devices to straighten teeth.";

            Treatment braces = TreatmentBuilder.CreateValid(treatmentName, treatmentBaseCost, treatmentDescription);

            // Act: Invoke the entity that is being tested
            Specialty specialty = SpecialtyBuilder.CreateActiveWithTreatments([braces], specialtyName, specialtyDescription);

            // Assert
            // Validate Aggregate: Specialty
            Assert.Equal(specialtyName, specialty.Name);
            Assert.Equal(specialtyDescription, specialty.Description?.Value);
            Assert.Equal(EntityStatus.Active, specialty.Status);
            Assert.Single(specialty.Treatments);

            // Validate Aggregate member: Treatment
            Treatment createdTreatment = specialty.Treatments.First();
            Assert.Equal(treatmentName, createdTreatment.Name);
            Assert.Equal(treatmentDescription, createdTreatment.Description?.Value);
            Assert.Equal(treatmentBaseCost, createdTreatment.BaseCost);
            Assert.Equal(EntityStatus.Active, createdTreatment.Status);
        }



        [Fact]
        public void Create_WhenEmptyTreatmentList_ShouldThrowEmptyTreatmentListException()
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
        public void Create_WhenTreatmentNamesAreDuplicatedInList_ShouldThrowDuplicateTreatmentNameException()
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
