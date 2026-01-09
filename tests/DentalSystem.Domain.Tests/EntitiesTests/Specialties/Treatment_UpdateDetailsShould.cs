using DentalSystem.Domain.Enums;
using DentalSystem.Domain.Exceptions.Specialties;
using DentalSystem.Domain.Tests.Builder;
using DentalSystem.Domain.Tests.Helpers;

namespace DentalSystem.Domain.Tests.EntitiesTests.Specialties
{
    public class Treatment_UpdateDetailsShould
    {
      //Happy path
        [Fact]
        public void UpdateDetails_WhenValidData_ShouldUpdateDetails_AndPreserveStatus()
        {
            // Arrange
            var treatment = TreatmentBuilder.CreateValid("Cleaning", 
                100, 
                "Original");

            // take snapshot
            Guid originalId = treatment.TreatmentId;
            string originalName = treatment.Name;
            string? originalDescription = treatment.Description ?? string.Empty;
            decimal originalBaseCost = treatment.BaseCost;

            decimal newBaseCost = 200;
            string newDescription = "Updated";
            // Act
            treatment.UpdateDetails(newBaseCost, newDescription);

            // Assert
            treatment.AssertInvariants(
               originalId,
               EntityStatus.Active,
               newBaseCost,
               originalName,
               newDescription
             );
        }

        // Idempotence

        [Fact]
        public void UpdateDetails_WhenDataIsIdentical_ShouldNotMutateAnyProperty()
        {
            // Arrange
            decimal baseCost = 100;
            string description = "Same description";

            var treatment = TreatmentBuilder.CreateValid("Cleaning",
                baseCost,
                description);

            // take snapshot
            Guid originalId = treatment.TreatmentId;
            string originalName = treatment.Name;
            string? originalDescription = treatment.Description ?? string.Empty;
            decimal originalBaseCost = treatment.BaseCost;

           
            // Act
            treatment.UpdateDetails(baseCost, description);

            // Assert
            // Data should remain the same
            treatment.AssertInvariants(
               originalId,
               EntityStatus.Active,
               baseCost,
               originalName,
               description
             );
        }

        // Errors
        [Fact]
        public void UpdateDetails_WhenBaseCostIsNegative_ShouldThrowInvalidTreatmentCostException()
        {
            // Arrange
            var treatment = TreatmentBuilder.CreateValid("Cleaning",
                100,
                "Treatment description");

            // take snapshot
            Guid originalId = treatment.TreatmentId;
            string originalName = treatment.Name;
            string? originalDescription = treatment.Description ?? string.Empty;
            decimal originalBaseCost = treatment.BaseCost;

            decimal invalidBaseCost = -50;

            // Act
            // Assert
            Assert.Throws<InvalidTreatmentCostException>(() => {
                treatment.UpdateDetails(invalidBaseCost);
            });

            // Data should remain the same
            treatment.AssertInvariants(
               originalId,
               EntityStatus.Active,
               originalBaseCost,
               originalName,
               originalDescription
             );
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

            // take snapshot
            Guid originalId = treatment.TreatmentId;
            string originalName = treatment.Name;
            string? originalDescription = treatment.Description ?? string.Empty;
            decimal originalBaseCost = treatment.BaseCost;

            // Act
            // Assert
            Assert.Throws<InvalidTreatmentDescriptionException>(() => {
                treatment.UpdateDetails(200, newDescription: invalidInput);
            });

            // Data should remain the same
            treatment.AssertInvariants(
               originalId,
               EntityStatus.Active,
               originalBaseCost,
               originalName,
               originalDescription
             );
        }
    }
}
