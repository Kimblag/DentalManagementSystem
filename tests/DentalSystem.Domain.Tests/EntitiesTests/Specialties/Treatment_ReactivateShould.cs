using DentalSystem.Domain.Exceptions.Specialties;
using DentalSystem.Domain.Tests.Builder;
using DentalSystem.Domain.Tests.Helpers;
using DentalSystem.Domain.ValueObjects;

namespace DentalSystem.Domain.Tests.EntitiesTests.Specialties
{
    public class Treatment_ReactivateShould
    {
        // Happy path
        [Fact]
        public void Reactivate_WhenTreatmentIsInactive_ShouldSetStatusToActive()
        {
            // Arrange
            var treatment = TreatmentBuilder.CreateValid();
            var expectedStatus = LifecycleStatus.Active();

            Guid originalId = treatment.TreatmentId;
            string originalName = treatment.Name;
            string? originalDescription = treatment.Description?.Value;
            decimal originalBaseCost = treatment.BaseCost;

            treatment.Deactivate();

            // Act
            treatment.Reactivate();

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
        public void Reactivate_WhenTreatmentIsAlreadyActive_ShouldThrowTreatmentAlreadyActiveException()
        {
            // Arrange
            var treatment = TreatmentBuilder.CreateValid();
            var expectedStatus = LifecycleStatus.Active();

            Guid originalId = treatment.TreatmentId;
            string originalName = treatment.Name;
            string? originalDescription = treatment.Description?.Value;
            decimal originalBaseCost = treatment.BaseCost;

            // Act
            // Assert
            Assert.Throws<InvalidStatusTransitionException>(() =>
            {
                treatment.Reactivate();
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
