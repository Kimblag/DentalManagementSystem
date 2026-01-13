using DentalSystem.Domain.Enums;
using DentalSystem.Domain.Exceptions.Specialties;
using DentalSystem.Domain.Tests.Builder;
using DentalSystem.Domain.Tests.Helpers;

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
            treatment.Deactivate();
            Guid originalId = treatment.TreatmentId;
            string originalName = treatment.Name;
            string? originalDescription = treatment.Description?.Value;
            decimal originalBaseCost = treatment.BaseCost;

            // Act
            treatment.Reactivate();

            // Assert
            treatment.AssertInvariants(
               originalId,
               EntityStatus.Active,
               originalBaseCost,
               originalName,
               originalDescription
             );
        }


        // Error
        [Fact]
        public void Reactivate_WhenTreatmentIsAlreadyActive_ShouldThrowTreatmentAlreadyActiveException()
        {
            // Arrange
            var treatment = TreatmentBuilder.CreateValid();
            
            Guid originalId = treatment.TreatmentId;
            string originalName = treatment.Name;
            string? originalDescription = treatment.Description?.Value;
            decimal originalBaseCost = treatment.BaseCost;

            // Act
            // Assert
            Assert.Throws<TreatmentAlreadyActiveException>(() =>
            {
                treatment.Reactivate();
            });

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
