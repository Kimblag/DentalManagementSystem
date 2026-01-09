using DentalSystem.Domain.Entities;
using DentalSystem.Domain.Enums;
using DentalSystem.Domain.Exceptions.Specialties;

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
            string treatmentName = "Braces";
            string treatmentDescription = "Metal or ceramic devices to straighten teeth.";
            decimal treatmentBaseCost = 10.0m;

            var treatments = new List<Treatment> {

                new Treatment(treatmentName, treatmentBaseCost, treatmentDescription)
            };

            string specialtyName = "Orthodontics";
            string specialtyDescription = "Focuses on correcting teeth and jaw alignment issues.";


            // Act: Invoke the entity that is being tested
            Specialty specialty = new Specialty(specialtyName, treatments, specialtyDescription);


            // Assert

            // Validate Aggregate: Specialty
            Assert.Equal(specialtyName, specialty.Name);
            Assert.Equal(specialtyDescription, specialty.Description);
            Assert.Equal(EntityStatus.Active, specialty.Status);
            Assert.Single(specialty.Treatments);

            // Validate Aggregate member: Treatment
            Treatment createdTreatment = specialty.Treatments.First();
            Assert.Equal(treatmentName, createdTreatment.Name);
            Assert.Equal(treatmentDescription, createdTreatment.Description);
            Assert.Equal(treatmentBaseCost, createdTreatment.BaseCost);
            Assert.Equal(EntityStatus.Active, createdTreatment.Status);
        }


        // multitest: we can pass multiple cases
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("or")]
        public void Create_WhenNameIsInvalid_ShouldThrowInvalidNameException(string? invalidName)
        {
            // Arrange
            string treatmentName = "Braces";
            string treatmentDescription = "Metal or ceramic devices to straighten teeth.";
            decimal treatmentBaseCost = 10.0m;

            var treatments = new List<Treatment> {

                new Treatment(treatmentName, treatmentBaseCost, treatmentDescription)
            };

            string specialtyDescription = "Focuses on correcting teeth and jaw alignment issues.";

            // Act and Assert
            Assert.Throws<InvalidSpecialtyNameException>(() =>
            {
                new Specialty(invalidName!, treatments, specialtyDescription);
            });
           
        }


        [Fact]
        public void Create_WhenEmptyTreatmentList_ShouldThrowEmptyTreatmentListException()
        {
            // Arrange
            var invalidTreatments = new List<Treatment>();

            string specialtyName = "Orthodontics";
            string specialtyDescription = "Focuses on correcting teeth and jaw alignment issues.";

            // Act and Assert
            Assert.Throws<EmptyTreatmentListException>(() =>
            {
                new Specialty(specialtyName, invalidTreatments, specialtyDescription);
            });
        }
        

        [Fact]
        public void Create_WhenTreatmentNamesAreDuplicatedInList_ShouldThrowDuplicateTreatmentNameException()
        {
            // Arrange
            string duplicateName = "Braces";

            var duplicateTreatments = new List<Treatment> {

                new Treatment(duplicateName, 10.0m, "Instance 1"),
                new Treatment(duplicateName, 20.0m, "Instance 2"),
            };

            string specialtyName = "Orthodontics";
            string specialtyDescription = "Focuses on correcting teeth and jaw alignment issues.";

            // Act and Assert
            Assert.Throws<DuplicateTreatmentNameException>(() =>
            {
                new Specialty(specialtyName, duplicateTreatments, specialtyDescription);
            });
        }


    }
}
