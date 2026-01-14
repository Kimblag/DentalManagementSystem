using DentalSystem.Domain.Entities;
using DentalSystem.Domain.Exceptions;
using DentalSystem.Domain.Tests.Builder;
using DentalSystem.Domain.Tests.Helpers;
using DentalSystem.Domain.ValueObjects;

namespace DentalSystem.Domain.Tests.EntitiesTests.Specialties
{
    public class Treatment_UpdateDetailsShould
    {
      //Happy path
        [Fact]
        public void UpdateDetails_WhenValidData_ShouldUpdateDetails_AndPreserveStatus()
        {
            // Arrange
            decimal newBaseCost = 200;
            string newDescription = new("Updated");
            var expectedStatus = LifecycleStatus.Active();

            Treatment treatment = TreatmentBuilder.CreateValid("Cleaning", 
                100, 
                "Original");

            // take snapshot
            Guid originalId = treatment.TreatmentId;
            string originalName = treatment.Name;

            // Act
            treatment.UpdateDetails(newBaseCost, new Description(newDescription));

            // Assert
            treatment.AssertInvariants(
               originalId,
               newBaseCost,
               originalName,
               newDescription
             );

            Assert.Equal(expectedStatus, treatment.Status);
        }

        // Idempotence

        [Fact]
        public void UpdateDetails_WhenDataIsIdentical_ShouldNotMutateAnyProperty()
        {
            // Arrange
            decimal baseCost = 100;
            string description = "Same description";
            var expectedStatus = LifecycleStatus.Active();

            var treatment = TreatmentBuilder.CreateValid("Cleaning",
                baseCost,
                description);

            // take snapshot
            Guid originalId = treatment.TreatmentId;
            string originalName = treatment.Name;


            // Act
            treatment.UpdateDetails(baseCost, new Description(description));

            // Assert
            // Data should remain the same
            treatment.AssertInvariants(
               originalId,
               baseCost,
               originalName,
               description
             );
            Assert.Equal(expectedStatus, treatment.Status);
        }

        // Errors
        [Fact]
        public void UpdateDetails_WhenBaseCostIsNegative_ShouldThrowInvalidTreatmentCostException()
        {
            // Arrange
            var treatment = TreatmentBuilder.CreateValid("Cleaning",
                100,
                "Treatment description");
            var expectedStatus = LifecycleStatus.Active();

            // take snapshot
            Guid originalId = treatment.TreatmentId;
            string originalName = treatment.Name;
            string? originalDescription = treatment.Description?.Value;
            decimal originalBaseCost = treatment.BaseCost;

            decimal invalidBaseCost = -50;

            // Act
            // Assert
            Assert.ThrowsAny<DomainException>(() => {
                treatment.UpdateDetails(invalidBaseCost);
            });

            // Data should remain the same
            treatment.AssertInvariants(
               originalId,
               originalBaseCost,
               originalName,
               originalDescription
             );

            Assert.Equal(expectedStatus, treatment.Status);
        }


        [Theory]
        [InlineData("a")]
        [InlineData("!@#$%")]
        public void UpdateDetails_WhenDescriptionIsInvalid_ShouldThrowInvalidTreatmentDescriptionException(string invalidInput)
        {
            // Arrange
            var treatment = TreatmentBuilder.CreateValid("Cleaning",
                100,
                "Original");
            var expectedStatus = LifecycleStatus.Active();

            // take snapshot
            Guid originalId = treatment.TreatmentId;
            string originalName = treatment.Name;
            string? originalDescription = treatment.Description?.Value;
            decimal originalBaseCost = treatment.BaseCost;

            // Act
            // Assert
            Assert.ThrowsAny<DomainException>(() => {
                treatment.UpdateDetails(200, newDescription: new Description(invalidInput));
            });

            // Data should remain the same
            treatment.AssertInvariants(
               originalId,
               originalBaseCost,
               originalName,
               originalDescription
             );
            Assert.Equal(expectedStatus, treatment.Status);
        }
    }
}
