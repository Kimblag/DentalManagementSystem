using DentalSystem.Domain.Entities;
using DentalSystem.Domain.Enums;
using DentalSystem.Domain.Exceptions.Specialties;
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
            Treatment treatment1 = TreatmentBuilder.CreateValid();
            Treatment treatment2 = TreatmentBuilder.CreateValid("Retainers", 10.0m, 
                "Devices used to maintain teeth position after treatment.");

            Specialty specialty = SpecialtyBuilder.CreateActiveWithTreatments(
            [
                treatment1,
                treatment2,
            ]);

            // Act
            specialty.DeactivateTreatment(treatment2.TreatmentId);

            // Assert
            Assert.True(treatment2.Status.IsInactive);
        }


        [Fact]
        public void DeactivateTreatment_WhenIsTheLastActiveTreatment_ShouldThrowMinimumSpecialtyTreatmentsException()
        {
            // Arrange
            Treatment treatment1 = TreatmentBuilder.CreateValid();

            Specialty specialty = SpecialtyBuilder.CreateActiveWithTreatments(
            [
                treatment1,
            ]);

            // Act
            // Assert
            Assert.Throws<MinimumSpecialtyTreatmentsException>(() =>
            {
                specialty.DeactivateTreatment(treatment1.TreatmentId);
            });
        }

        [Fact]
        public void DeactivateTreatment_WhenSpecialtyIsNotActive_ShouldThrowInvalidSpecialtyStateException() 
        {
            // Arrange
            Treatment treatment1 = TreatmentBuilder.CreateValid();
            Treatment treatment2 = TreatmentBuilder.CreateValid("Retainers", 10.0m, 
                "Devices used to maintain teeth position after treatment.");

            Specialty specialty = SpecialtyBuilder.CreateActiveWithTreatments(
            [
                treatment1,
                treatment2,
            ]);

            specialty.Deactivate();

            // Act
            // Assert
            Assert.Throws<InvalidSpecialtyStateException>(() =>
            {
                specialty.DeactivateTreatment(treatment1.TreatmentId);
            });
        }


        [Fact]
        public void DeactivateTreatment_WhenTreatmentIdIsNotFound_ShouldThrowTreatmentNotFoundException() 
        {
            // Arrange
            Treatment treatment1 = TreatmentBuilder.CreateValid();
            Treatment treatment2 = TreatmentBuilder.CreateValid("Retainers", 10.0m, "Devices used to maintain teeth position after treatment.");

            Specialty specialty = SpecialtyBuilder.CreateActiveWithTreatments(
            [
                treatment1,
                treatment2,
            ]);

            // Act
            // Assert
            Assert.Throws<TreatmentNotFoundException>(() =>
            {
                specialty.DeactivateTreatment(Guid.NewGuid());
            });
        }


        [Fact]
        public void DeactivateTreatment_WhenTreatmentIsAlreadyInactive_ShouldThrowTreatmentAlreadyInactiveException()
        {
            // Arrange
            Treatment treatment1 = TreatmentBuilder.CreateValid();
            Treatment treatment2 = TreatmentBuilder.CreateValid("Retainers", 20.0m, 
                "Devices used to maintain teeth position after treatment.");
            Treatment inactivetreatment = TreatmentBuilder.CreateValid("Clear Aligners", 25.0m, 
                "Removable transparent aligners for teeth correction.");

            Specialty specialty = SpecialtyBuilder.CreateActiveWithTreatments(
            [
                treatment1,
                treatment2,
                inactivetreatment,
            ]);

            specialty.DeactivateTreatment(inactivetreatment.TreatmentId);

            // Act
            // Assert
            Assert.Throws<TreatmentAlreadyInactiveException>(() =>
            {
                specialty.DeactivateTreatment(inactivetreatment.TreatmentId);
            });
        }

    }
}
