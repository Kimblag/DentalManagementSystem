using DentalSystem.Domain.Exceptions.Specialties;
using DentalSystem.Domain.Tests.Builder;
using DentalSystem.Domain.Tests.Helpers;
using DentalSystem.Domain.ValueObjects;

namespace DentalSystem.Domain.Tests.EntitiesTests.Specialties
{
    public class Treatment_DeactivateShould
    {
        // happy path
        [Fact]
        public void Deactivate_WhenTreatmentIsActive_ShouldSetStatusToInactive()
        {
            // Arrange
            var treatment = TreatmentBuilder.CreateValid();
            var expectedStatus = LifecycleStatus.Inactive();

            Guid originalId = treatment.TreatmentId;
            string originalName = treatment.Name;
            string? originalDescription = treatment.Description?.Value;
            decimal originalBaseCost = treatment.BaseCost;

            // Act
            treatment.Deactivate();

            // Assert
            treatment.AssertInvariants(
                originalId,
                originalBaseCost,
                originalName,
                originalDescription
                );
            Assert.Equal(expectedStatus, treatment.Status);
        }


        // Error
        [Fact]
        public void Deactivate_WhenTreatmentIsAlreadyInactive_ShouldThrowException_AndPreserveState()
        {
            // Arrange
            var treatment = TreatmentBuilder.CreateValid();
            var expectedStatus = LifecycleStatus.Inactive();

            treatment.Deactivate();
            
            // Take snapshot
            Guid originalId = treatment.TreatmentId;
            string originalName = treatment.Name;
            string? originalDescription = treatment.Description?.Value;
            decimal originalBaseCost = treatment.BaseCost;

            // Act 
            // Assert
            Assert.Throws<InvalidStatusTransitionException>(() =>
            {
                treatment.Deactivate();
            });
            
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
