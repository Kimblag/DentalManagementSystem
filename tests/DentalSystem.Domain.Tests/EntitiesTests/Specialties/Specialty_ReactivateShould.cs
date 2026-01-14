using DentalSystem.Domain.Entities;
using DentalSystem.Domain.Exceptions.Specialties;
using DentalSystem.Domain.Tests.Builder;
using DentalSystem.Domain.Tests.Helpers;
using DentalSystem.Domain.ValueObjects;

namespace DentalSystem.Domain.Tests.EntitiesTests.Specialties
{
    public class Specialty_ReactivateShould
    {
        // Happy path
        [Fact]
        public void Reactivate_WhenSpecialtyIsInactive_ShouldSetStatusToActive_AndCascadeToTreatments()
        {
            // Arrange
            var specialty = SpecialtyBuilder.CreateActiveWithMultipleTreatments();
            specialty.Deactivate();

            // snapshot
            Guid originalId = specialty.SpecialtyId;
            string? originalDescription = specialty.Description?.Value;
            string originalName = specialty.Name.Value;
            var expectedTreatmentIds = specialty.Treatments.Select(t => t.TreatmentId).ToList();

            // Act
            specialty.Reactivate();

            // Assert
            Assert.True(specialty.Status.IsActive);
            Assert.All(specialty.Treatments, t => Assert.True(t.Status.IsActive));

            // check for invariants (what did NOT change)
            Assert.Equal(originalId, specialty.SpecialtyId);
            Assert.Equal(originalName, specialty.Name);
            Assert.Equal(originalDescription, specialty.Description?.Value);

            // verify that the treatments are still the same (by ID)
            Assert.Equal(expectedTreatmentIds, [.. specialty.Treatments.Select(t => t.TreatmentId)]);
        }


        // errors
        [Fact]
        public void Reactivate_WhenSpecialtyIsAlreadyActive_ShouldThrowInvalidStatusTransitionException_AndPreserveState()
        {
            // Arrange
            var specialty = SpecialtyBuilder.CreateActiveWithMultipleTreatments();
            var expectedStatus = LifecycleStatus.Active();

            // snapshot
            Guid originalId = specialty.SpecialtyId;
            string? originalDescription = specialty.Description?.Value;
            string originalName = specialty.Name;
            List<Treatment> originalTreatments = [.. specialty.Treatments];

            // Act
            // Assert
            Assert.Throws<InvalidStatusTransitionException>(() =>
            {
                specialty.Reactivate();
            });

            specialty.AssertInvariants(
              originalId,
              originalTreatments,
              originalName,
              originalDescription
              );
            Assert.Equal(expectedStatus, specialty.Status);

        }

    }
}
