using DentalSystem.Domain.Entities;
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
            Assert.True(specialty.Status.IsInactive);

            Assert.All(specialty.Treatments, t =>
                Assert.True(t.Status.IsInactive));
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
