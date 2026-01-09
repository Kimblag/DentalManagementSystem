using DentalSystem.Domain.Entities;
using DentalSystem.Domain.Enums;
using DentalSystem.Domain.Exceptions.Specialties;
using DentalSystem.Domain.Tests.Builder;

namespace DentalSystem.Domain.Tests.EntitiesTests.Specialties
{
    public class Specialty_RemoveTreatmentShould
    {
        // Happy path
        [Fact]
        public void RemoveTreatment_WhenValidIdAndMultipleExist_ShouldSetTreatmentToInactive() 
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
            specialty.RemoveTreatment(treatment2.TreatmentId);

            // Assert
            Assert.Equal(EntityStatus.Inactive, treatment2.Status);
        }


        [Fact]
        public void RemoveTreatment_WhenIsTheLastActiveTreatment_ShouldThrowMinimumSpecialtyTreatmentsException()
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
                specialty.RemoveTreatment(treatment1.TreatmentId);
            });
        }

        [Fact]
        public void RemoveTreatment_WhenSpecialtyIsNotActive_ShouldThrowInvalidSpecialtyStateException() 
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
                specialty.RemoveTreatment(treatment1.TreatmentId);
            });
        }


        [Fact]
        public void RemoveTreatment_WhenTreatmentIdIsNotFound_ShouldThrowTreatmentNotFoundException() 
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
                specialty.RemoveTreatment(Guid.NewGuid());
            });
        }


        [Fact]
        public void RemoveTreatment_WhenTreatmentIsAlreadyInactive_ShouldThrowTreatmentAlreadyInactiveException()
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

            specialty.RemoveTreatment(inactivetreatment.TreatmentId);

            // Act
            // Assert
            Assert.Throws<TreatmentAlreadyInactiveException>(() =>
            {
                specialty.RemoveTreatment(inactivetreatment.TreatmentId);
            });
        }

    }
}
