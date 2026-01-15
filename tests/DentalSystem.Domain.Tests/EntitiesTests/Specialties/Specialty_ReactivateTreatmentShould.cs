using DentalSystem.Domain.Entities;
using DentalSystem.Domain.Exceptions.Specialties;
using DentalSystem.Domain.Tests.Builder;

namespace DentalSystem.Domain.Tests.EntitiesTests.Specialties
{
    public class Specialty_ReactivateTreatmentShould
    {
        // Happy path
        [Fact]
        public void ReactivateTreatment_WhenValidId_ShouldSetTreatmentToActive() 
        {
            // Arrange
            Specialty specialty = SpecialtyBuilder.CreateActiveWithTwoDistinctTreatmentsAndOneInactiveTreatment();

            // get the first treatment
            Treatment treatmentToReactivate = specialty.Treatments.First();
            Guid treatmentToReactivateId = specialty.Treatments.First().TreatmentId;

            // Act
            specialty.ReactivateTreatment(treatmentToReactivateId);

            // Assert
            Assert.True(treatmentToReactivate.Status.IsActive);
        }



        [Fact]
        public void ReactivateTreatment_WhenSpecialtyIsNotActive_ShouldThrowInvalidSpecialtyStateException() 
        {
            // Arrange
            Specialty specialty = SpecialtyBuilder.CreateInactive();

            Treatment treatmentToReactivate = specialty.Treatments.First();
            Guid treatmentToReactivateId = specialty.Treatments.First().TreatmentId;

            // Act
            // Assert
            Assert.Throws<InvalidSpecialtyStateException>(() =>
            {
                specialty.ReactivateTreatment(treatmentToReactivateId);
            });
        }


        [Fact]
        public void ReactivateTreatment_WhenTreatmentIdIsNotFound_ShouldThrowTreatmentNotFoundException() 
        {
            // Arrange
            Specialty specialty = SpecialtyBuilder.CreateActiveWithTwoDistinctTreatmentsAndOneInactiveTreatment();

            // Act
            // Assert
            Assert.Throws<TreatmentNotFoundException>(() =>
            {
                specialty.ReactivateTreatment(Guid.NewGuid());
            });
        }


        [Fact]
        public void ReactivateTreatment_WhenTreatmentIsAlreadyActive_ShouldThrowInvalidStatusTransitionException()
        {
            // Arrange
            Specialty specialty = SpecialtyBuilder.CreateActiveWithTwoDistinctTreatments();

            Treatment treatmentToReactivate = specialty.Treatments.First();
            Guid treatmentToReactivateId = specialty.Treatments.First().TreatmentId;

            // Act
            // Assert
            Assert.Throws<InvalidStatusTransitionException>(() =>
            {
                specialty.ReactivateTreatment(treatmentToReactivateId);
            });
        }

    }
}
