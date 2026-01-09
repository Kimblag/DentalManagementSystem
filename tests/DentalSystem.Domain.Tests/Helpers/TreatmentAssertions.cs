using DentalSystem.Domain.Entities;
using DentalSystem.Domain.Enums;

namespace DentalSystem.Domain.Tests.Helpers
{
    public static class TreatmentAssertions
    {
        public static void AssertInvariants(
            this Treatment treatment,
            Guid expectedId,
            EntityStatus expectedStatus,
            decimal? expectedBaseCost = null,
            string? expectedName = null,
            string? expectedDescription = null)
        {
            // Identity should never change
            Assert.Equal(expectedId, treatment.TreatmentId);

            // Expected status. Depends on the operation applied
            Assert.Equal(expectedStatus, treatment.Status);

            // Expected base cost
            Assert.Equal(expectedBaseCost, treatment.BaseCost);

            if (expectedName != null)
                Assert.Equal(expectedName, treatment.Name);

            if (expectedDescription != null)
                Assert.Equal(expectedDescription, treatment.Description);
        }
    }
}
