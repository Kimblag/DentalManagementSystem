using DentalSystem.Domain.Entities;
using DentalSystem.Domain.Enums;
using DentalSystem.Domain.Exceptions.Specialties;
using DentalSystem.Domain.Tests.Builder;

namespace DentalSystem.Domain.Tests.EntitiesTests.Specialties
{
    public class Specialty_DeactivateShould
    {
        // happy path
        [Fact]
        public void Deactivate_WhenSpecialtyIsActive_ShouldSetStatusToInactiveAndCascadeToTreatments()
        {
            // Arrange
            Specialty specialty = SpecialtyBuilder.CreateActiveWithOneTreatment();

            // Act
            specialty.Deactivate();

            //Assert
            Assert.Equal(EntityStatus.Inactive, specialty.Status);

            Assert.All(specialty.Treatments, t =>
                Assert.Equal(EntityStatus.Inactive, t.Status));
        }


        // errors
        [Fact]
        public void Deactivate_WhenSpecialtyIsAlreadyInactive_ShouldThrowInvalidStatusTransitionException()
        {
            // Arrange
            Specialty specialty = SpecialtyBuilder.CreateActiveWithOneTreatment();

            // Act
            specialty.Deactivate();

            // Assert
            Assert.Throws<InvalidStatusTransitionException>(() =>
            {
                specialty.Deactivate();
            });
        }
    }
}
