using DentalSystem.Domain.Enums;
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
            Guid originalId = treatment.TreatmentId;
            string originalName = treatment.Name;
            string? originalDescription = treatment.Description?.Value;
            decimal originalBaseCost = treatment.BaseCost;
            LifecycleStatus originalStatus = treatment.Status;

            // Act
            treatment.Deactivate();

            // Assert
            treatment.AssertInvariants(
                originalId,
                originalStatus,
                originalBaseCost,
                originalName,
                originalDescription
                );
        }


        // Error
        [Fact]
        public void Deactivate_WhenTreatmentIsAlreadyInactive_ShouldThrowException_AndPreserveState()
        {
            // Arrange
            var treatment = TreatmentBuilder.CreateValid();
            treatment.Deactivate();

            // Take snapshot
            Guid originalId = treatment.TreatmentId;
            string originalName = treatment.Name;
            string? originalDescription = treatment.Description?.Value;
            decimal originalBaseCost = treatment.BaseCost;
            LifecycleStatus originalStatus = treatment.Status;

            // Act 
            // Assert
            Assert.Throws<TreatmentAlreadyInactiveException>(() =>
            {
                treatment.Deactivate();
            });
            
            treatment.AssertInvariants(
               originalId,
               originalStatus,
               originalBaseCost,
               originalName,
               originalDescription
               );
        }
    }
}
