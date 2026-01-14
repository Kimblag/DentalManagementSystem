using DentalSystem.Domain.Entities;
using DentalSystem.Domain.ValueObjects;
using DentalSystem.Domain.Exceptions;
using DentalSystem.Domain.Exceptions.Specialties;
using DentalSystem.Domain.Tests.Builder;

namespace DentalSystem.Domain.Tests.EntitiesTests.Specialties
{
    public class Specialty_UpdateTreatmentDetailsShould
    {
        // HAppy path
        [Fact]
        public void UpdateTreatmentDetails_WhenValidData_ShouldUpdateTreatment()
        {
            // Arrange
            Treatment treatment = TreatmentBuilder.CreateValid(
                 name: "Cleaning",
                 baseCost: 50,
                 description: "Basic cleaning"
             );

            Specialty specialty = SpecialtyBuilder.CreateActiveWithTreatments(
                [treatment],
                specialtyName: "General Dentistry",
                specialtyDescription: "General treatments"
            );

            decimal newCost = 80;
            string newDescription = "Deep cleaning";
            string newName = "Deep Cleaning";

            // Act
            specialty.UpdateTreatmentDetails(
                treatmentId: treatment.TreatmentId,
                treatmentBaseCost: newCost,
                treatmentDescription: new Description(newDescription),
                treatmentName: newName
            );

            // Assert
            Treatment updatedTreatment = specialty.Treatments.Single();

            Assert.Equal(newName, updatedTreatment.Name);
            Assert.Equal(newCost, updatedTreatment.BaseCost);
            Assert.Equal(newDescription, updatedTreatment.Description?.Value);
            Assert.True(updatedTreatment.Status.IsActive);
        }


        // Errors

        // inactive specialty
        [Fact]
        public void UpdateTreatmentDetails_WhenSpecialtyIsInactive_ShouldThrowInvalidSpecialtyStateException()
        {
            // Arrange
            Treatment treatment = TreatmentBuilder.CreateValid(
                 "Cleaning",
                 50,
                 "Basic cleaning"
             );

            Specialty specialty = SpecialtyBuilder.CreateActiveWithTreatments(
                [treatment],
                "General Dentistry"
            );

            specialty.Deactivate();

            // Act
            // Assert
            Assert.Throws<InvalidSpecialtyStateException>(() =>
                specialty.UpdateTreatmentDetails(
                    treatment.TreatmentId,
                    treatmentBaseCost: 100
                )
            );
        }

        // Treatment not found
        [Fact]
        public void UpdateTreatmentDetails_WhenTreatmentDoesNotExist_ShouldThrowTreatmentNotFoundException()
        {
            // Arrange
            Treatment treatment = TreatmentBuilder.CreateValid(
                "Cleaning",
                50,
                "Basic cleaning"
            );

            Specialty specialty = SpecialtyBuilder.CreateActiveWithTreatments(
                [treatment],
                "General Dentistry"
            );

            Guid nonExistingTreatmentId = Guid.NewGuid();

            // Act
            // Assert
            Assert.Throws<TreatmentNotFoundException>(() =>
                specialty.UpdateTreatmentDetails(
                    nonExistingTreatmentId,
                    treatmentBaseCost: 100
                )
            );
        }


        // Treatment inactive
        [Fact]
        public void UpdateTreatmentDetails_WhenTreatmentIsInactive_ShouldThrowInvalidTreatmentStateException()
        {
            // Arrange
            Treatment treatment = TreatmentBuilder.CreateValid(
                "Cleaning",
                50,
                "Basic cleaning"
            );

            Specialty specialty = SpecialtyBuilder.CreateActiveWithTreatments(
                [treatment],
                "General Dentistry"
            );

            treatment.Deactivate();

            // Act
            // Assert
            Assert.Throws<InvalidTreatmentStateException>(() =>
                specialty.UpdateTreatmentDetails(
                    treatment.TreatmentId,
                    treatmentBaseCost: 100
                )
            );
        }


        [Fact]
        public void UpdateTreatmentDetails_WhenNameIsInvalid_ShouldNotApplyAnyChanges()
        {
            // Arrange
            Treatment treatment = TreatmentBuilder.CreateValid(
                "Cleaning",
                50,
                "Basic cleaning"
            );

            Specialty specialty = SpecialtyBuilder.CreateActiveWithTreatments(
                [treatment],
                "General Dentistry"
            );

            string originalName = treatment.Name;
            decimal originalCost = treatment.BaseCost;
            string? originalDescription = treatment.Description?.Value;

            // Act
            // Assert
            Assert.ThrowsAny<DomainException>(() =>
                specialty.UpdateTreatmentDetails(
                    treatment.TreatmentId,
                    treatmentName: "!"
                )
            );

            // no partial mutation
            Treatment unchanged = specialty.Treatments.Single();

            Assert.Equal(originalName, unchanged.Name);
            Assert.Equal(originalCost, unchanged.BaseCost);
            Assert.Equal(originalDescription, unchanged.Description?.Value);
        }
    } 
}