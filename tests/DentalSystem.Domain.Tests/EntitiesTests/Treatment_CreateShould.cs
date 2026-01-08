using Xunit;

using DentalSystem.Domain.Entities;
using DentalSystem.Domain.Enums;
using DentalSystem.Domain.Exceptions;

namespace DentalSystem.Domain.Tests.EntitiesTests
{
    public class Treatment_CreateShould
    {

        [Fact]
        public void Create_WhenDataIsValid_ShouldCreateTreatmentSuccessfully()
        {
            // Arrange
            string treatmentName = "Braces";
            string treatmentDescription = "Metal or ceramic devices to straighten teeth.";
            decimal treatmentBaseCost = 10.0m;

            // Act: Invoke the entity that is being tested
            Treatment treatment = new Treatment(treatmentName, treatmentBaseCost, treatmentDescription);

            // Assert
            // Validate Aggregate member: Treatment
            Assert.Equal(treatmentName, treatment.Name);
            Assert.Equal(treatmentDescription, treatment.Description);
            Assert.Equal(treatmentBaseCost, treatment.BaseCost);
            Assert.Equal(EntityStatus.Active, treatment.Status);
        }


        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("or")]
        public void Create_WhenTreatmentNameIsInvalid_ShouldThrowInvalidNameException(string? invalidName)
        {
            // Arrange
            string treatmentDescription = "Metal or ceramic devices to straighten teeth.";
            decimal treatmentBaseCost = 10.0m;

            // Act and Assert
            Assert.Throws<InvalidTreatmentNameException>(() =>
            {
                new Treatment(invalidName!, treatmentBaseCost, treatmentDescription);
            });
        }


        [Fact]
        public void Create_WhenTreatmentBaseCostIsInvalid_ShouldThrowInvalidTreatmentCostException()
        {
            // Arrange
            string treatmentName = "Braces";
            string treatmentDescription = "Metal or ceramic devices to straighten teeth.";
            decimal invalidTreatmentBaseCost = -100;

            // Act and Assert
            Assert.Throws<InvalidTreatmentCostException>(() =>
            {
                new Treatment(treatmentName, invalidTreatmentBaseCost, treatmentDescription);
            });
        }

    }
}
