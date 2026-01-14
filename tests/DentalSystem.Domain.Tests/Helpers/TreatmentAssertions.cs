using DentalSystem.Domain.Entities;
using DentalSystem.Domain.Enums;
using DentalSystem.Domain.ValueObjects;

namespace DentalSystem.Domain.Tests.Helpers
{
    public static class TreatmentAssertions
    {
        public static void AssertInvariants(
            this Treatment treatment,
            Guid expectedId,
            LifecycleStatus expectedStatus,
            decimal? expectedBaseCost = null,
            string? expectedName = null,
            string? expectedDescription = null)
        {
            // Identity should never change
            Assert.Equal(expectedId, treatment.TreatmentId);

            // Expected status. Depends on the operation applied
            Assert.Equal(expectedStatus.IsActive, treatment.Status.IsActive);
            Assert.Equal(expectedStatus.IsInactive, treatment.Status.IsInactive);

            // Expected base cost
            Assert.Equal(expectedBaseCost, treatment.BaseCost);

            if (expectedName != null)
                Assert.Equal(expectedName, treatment.Name);

            if (expectedDescription != null)
                Assert.Equal(expectedDescription, treatment.Description?.Value);
        }
    }
}
