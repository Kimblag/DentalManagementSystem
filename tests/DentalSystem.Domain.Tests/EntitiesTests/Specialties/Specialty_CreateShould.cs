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
            string treatmentName = "Clear Aligners";
            decimal treatmentBaseCost = 25;
            string treatmentDescription = "Removable transparent aligners.";

            IEnumerable<(string name, decimal baseCost, string? description)> treatments =
            [
                (treatmentName, treatmentBaseCost, treatmentDescription),
            ];

            // Act
            Specialty specialty = new(specialtyName, treatments, specialtyDescription);

            // Assert
            Assert.Equal(specialtyName, specialty.Name.Value);
            Assert.Equal(specialtyDescription, specialty.Description?.Value);
            Assert.True(specialty.Status.IsActive);

            Assert.Single(specialty.Treatments);
            Assert.Contains(specialty.Treatments, t => t.Name.Value == treatmentName);
        }



        [Fact]
        public void Create_WhenTreatmentListIsEmpty_ShouldThrowDomainException()
        {
            // Arrange
            string specialtyName = "Orthodontics";
            string specialtyDescription = "Focuses on correcting teeth and jaw alignment issues.";

            IEnumerable<(string name, decimal baseCost, string? description)> invalidTreatments = [];

            // Act and Assert
            Assert.ThrowsAny<DomainException>(() =>
            {
               _ = new Specialty(specialtyName, invalidTreatments, specialtyDescription);
            });
        }
        

        [Fact]
        public void Create_WhenTreatmentNamesAreDuplicated_ShouldThrowDomainException()
        {
            // Arrange
            string duplicateTreatmentName = "Braces";

            string specialtyName = "Orthodontics";
            string specialtyDescription = "Focuses on correcting teeth and jaw alignment issues.";

            decimal treatmentBaseCost = 25;
            decimal treatmentBaseCost2 = 12;
            string treatmentDescription = "Removable transparent aligners.";

            IEnumerable<(string name, decimal baseCost, string? description)> invalidTreatments =
            [
                (duplicateTreatmentName, treatmentBaseCost, treatmentDescription),
                (duplicateTreatmentName, treatmentBaseCost2, null),
            ];

            // Act and Assert
            Assert.ThrowsAny<DomainException>(() =>
            {
                new Specialty(specialtyName, invalidTreatments, specialtyDescription);
            });
        }


    }
}
