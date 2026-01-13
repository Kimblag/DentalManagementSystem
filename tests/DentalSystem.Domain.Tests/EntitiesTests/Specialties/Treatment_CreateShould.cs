using DentalSystem.Domain.Entities;
using DentalSystem.Domain.Enums;
using DentalSystem.Domain.Exceptions;
using DentalSystem.Domain.Exceptions.Specialties;
using DentalSystem.Domain.ValueObjects;
using DentalSystem.Domain.Tests.Builder;

namespace DentalSystem.Domain.Tests.EntitiesTests.Specialties
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
            Treatment treatment = TreatmentBuilder.CreateValid(treatmentName, treatmentBaseCost, treatmentDescription);

            // Assert
            // Validate Aggregate member: Treatment
            Assert.Equal(treatmentName, treatment.Name);
            Assert.Equal(treatmentDescription, treatment.Description?.Value);
            Assert.Equal(treatmentBaseCost, treatment.BaseCost);
            Assert.Equal(EntityStatus.Active, treatment.Status);
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
                new Treatment(new Name(treatmentName), invalidTreatmentBaseCost, new Description(treatmentDescription));
            });
        }

    }
}
