using DentalSystem.Domain.Entities;
using DentalSystem.Domain.Enums;

namespace DentalSystem.Domain.Tests.Helpers
{
    public static class SpecialtyAssertions
    {
        public static void AssertInvariants(
            this Specialty specialty, // this converts to extension
            Guid expectedId,
            EntityStatus expectedStatus,
            List<Treatment> originalTreatments,
            string? expectedName = null,
            string? expectedDescription = null)
        {
            // Identity should never change
            Assert.Equal(expectedId, specialty.SpecialtyId);

            // Expected status. Depends on the operation applied
            Assert.Equal(expectedStatus, specialty.Status);

            // Collection integrity
            // The same quantity
            Assert.Equal(originalTreatments.Count, specialty.Treatments.Count);

            // The same instances in memory (exact references)
            // Protects against recreate objects or loss of references
            Assert.All(originalTreatments, t =>
                Assert.Contains(t, specialty.Treatments));

            // if ther are passed, optional data validation
            if (expectedName != null)
                Assert.Equal(expectedName, specialty.Name);

            if (expectedDescription != null)
                Assert.Equal(expectedDescription, specialty.Description?.Value);
        }
    }
}
