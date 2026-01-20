using DentalSystem.Domain.Entities;
using DentalSystem.Domain.Exceptions.Rules.Specialties;
using DentalSystem.Domain.Tests.Builder;

namespace DentalSystem.Domain.Tests.EntitiesTests.Specialties
{
    public class Specialty_DeactivateTreatmentShould
    {
        // Happy path
        [Fact]
        public void DeactivateTreatment_WhenValidIdAndMultipleExist_ShouldSetTreatmentToInactive() 
        {
            // Arrange
            Specialty specialty = SpecialtyBuilder.CreateActiveWithTwoDistinctTreatments();

            // get the first treatment
            Treatment treatmentToDeactivate = specialty.Treatments.First();
            Guid treatmentToDeactivateId = specialty.Treatments.First().TreatmentId;

            // Act
            specialty.DeactivateTreatment(treatmentToDeactivateId);

            // Assert
            Assert.True(treatmentToDeactivate.Status.IsInactive);
        }


        [Fact]
        public void DeactivateTreatment_WhenIsTheLastActiveTreatment_ShouldThrowMinimumSpecialtyTreatmentsException()
        {
            // Arrange
            Specialty specialty = SpecialtyBuilder.CreateActiveWithOneTreatment();

            Treatment treatmentToDeactivate = specialty.Treatments.First();
            Guid treatmentToDeactivateId = specialty.Treatments.First().TreatmentId;

            // Act
            // Assert
            Assert.Throws<MinimumSpecialtyTreatmentsException>(() =>
            {
                specialty.DeactivateTreatment(treatmentToDeactivateId);
            });
        }


        [Fact]
        public void DeactivateTreatment_WhenSpecialtyIsNotActive_ShouldThrowInvalidSpecialtyStateException() 
        {
            // Arrange
            Specialty specialty = SpecialtyBuilder.CreateInactive();

            Treatment treatmentToDeactivate = specialty.Treatments.First();
            Guid treatmentToDeactivateId = specialty.Treatments.First().TreatmentId;

            // Act
            // Assert
            Assert.Throws<InvalidSpecialtyStateException>(() =>
            {
                specialty.DeactivateTreatment(treatmentToDeactivateId);
            });
        }


        [Fact]
        public void DeactivateTreatment_WhenTreatmentIdIsNotFound_ShouldThrowTreatmentNotFoundException() 
        {
            // Arrange
            Specialty specialty = SpecialtyBuilder.CreateActiveWithTwoDistinctTreatments();

            // Act
            // Assert
            Assert.Throws<TreatmentNotFoundException>(() =>
            {
                specialty.DeactivateTreatment(Guid.NewGuid());
            });
        }


        [Fact]
        public void DeactivateTreatment_WhenTreatmentIsAlreadyInactive_ShouldThrowInvalidStatusTransitionExceptionException()
        {
            // Arrange
            Specialty specialty = SpecialtyBuilder.CreateActiveWithTwoDistinctTreatments();

            Guid treatmentToDeactivateId = specialty.Treatments.First().TreatmentId;
            specialty.DeactivateTreatment(treatmentToDeactivateId);

            // Act
            // Assert
            Assert.Throws<InvalidStatusTransitionException>(() =>
            {
                specialty.DeactivateTreatment(treatmentToDeactivateId);
            });
        }

    }
}
