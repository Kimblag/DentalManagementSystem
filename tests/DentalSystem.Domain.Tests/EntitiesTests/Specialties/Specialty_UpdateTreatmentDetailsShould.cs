using DentalSystem.Domain.Entities;
using DentalSystem.Domain.Exceptions;
using DentalSystem.Domain.Exceptions.Rules.Specialties;
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
            Specialty specialty = SpecialtyBuilder.CreateActiveWithOneTreatment();

            Guid treatmentId = specialty.Treatments.First().TreatmentId;

            string correctedName = "Braces Treatment";
            decimal newCost = 80;
            string newDescription = "Deep cleaning";

            // Act
            specialty.CorrectTreatmentName(treatmentId, correctedName);
            specialty.UpdateTreatmentDescription(treatmentId, newDescription);
            specialty.ChangeTreatmentBaseCost(treatmentId, newCost);

            // Assert
            Treatment updatedTreatment = specialty.Treatments.Single();

            Assert.Equal(correctedName, updatedTreatment.Name.Value);
            Assert.Equal(newCost, updatedTreatment.BaseCost);
            Assert.Equal(newDescription, updatedTreatment.Description?.Value);

            // Check it still is Active
            Assert.True(updatedTreatment.Status.IsActive);
        }


        // Errors

        // inactive specialty
        [Theory]
        [InlineData("CorrectName")]
        [InlineData("UpdateDescription")]
        [InlineData("ChangeBaseCost")]
        public void WhenSpecialtyIsInactive_ShouldNotAllowTreatmentModification(string operation)
        {
            Specialty specialty = SpecialtyBuilder.CreateInactive();
            Guid treatmentId = specialty.Treatments.First().TreatmentId;

            Action act = operation switch
            {
                "CorrectName" => () => specialty.CorrectTreatmentName(treatmentId, "Braces Treatment"),
                "UpdateDescription" => () => specialty.UpdateTreatmentDescription(treatmentId, "Deep cleaning"),
                "ChangeBaseCost" => () => specialty.ChangeTreatmentBaseCost(treatmentId, 80m),
                _ => throw new NotSupportedException()
            };

            Assert.Throws<InvalidSpecialtyStateException>(act);
        }

        // Treatment not found
        [Theory]
        [InlineData("CorrectName")]
        [InlineData("UpdateDescription")]
        [InlineData("ChangeBaseCost")]
        public void UpdateTreatmentDetails_WhenTreatmentDoesNotExist_ShouldThrowTreatmentNotFoundException(string operation)
        {
            // Arrange
            Specialty specialty = SpecialtyBuilder.CreateActiveWithOneTreatment();

            Action act = operation switch
            {
                "CorrectName" => () => specialty.CorrectTreatmentName(Guid.NewGuid(), "Braces Treatment"),
                "UpdateDescription" => () => specialty.UpdateTreatmentDescription(Guid.NewGuid(), "Deep cleaning"),
                "ChangeBaseCost" => () => specialty.ChangeTreatmentBaseCost(Guid.NewGuid(), 80m),
                _ => throw new NotSupportedException()
            };

            // Act
            // Assert
            Assert.Throws<TreatmentNotFoundException>(act);
        }


        // Treatment inactive
        [Theory]
        [InlineData("CorrectName")]
        [InlineData("UpdateDescription")]
        [InlineData("ChangeBaseCost")]
        public void UpdateTreatmentDetails_WhenTreatmentIsInactive_ShouldThrowInvalidTreatmentStateException(string operation)
        {
            // Arrange
            Specialty specialty = SpecialtyBuilder.CreateActiveWithTwoDistinctTreatments();
            Guid treatmentId = specialty.Treatments.First().TreatmentId;

            specialty.DeactivateTreatment(treatmentId);

            Action act = operation switch
            {
                "CorrectName" => () => specialty.CorrectTreatmentName(treatmentId, "Braces Treatment"),
                "UpdateDescription" => () => specialty.UpdateTreatmentDescription(treatmentId, "Deep cleaning"),
                "ChangeBaseCost" => () => specialty.ChangeTreatmentBaseCost(treatmentId, 80m),
                _ => throw new NotSupportedException()
            };

            // Act
            // Assert
            Assert.Equal(2, specialty.Treatments.Count);
            Assert.Throws<InvalidTreatmentStateException>(act);
        }


        [Theory]
        [InlineData("CorrectName")]
        [InlineData("UpdateDescription")]
        [InlineData("ChangeBaseCost")]
        public void UpdateTreatmentDetails_WhenDataIsInvalid_ShouldNotApplyAnyChanges(string operation)
        {
            // Arrange
            Specialty specialty = SpecialtyBuilder.CreateActiveWithOneTreatment();
            Treatment treatment = specialty.Treatments.First();
            Guid treatmentId = treatment.TreatmentId;

            string originalName = treatment.Name.Value;
            decimal originalCost = treatment.BaseCost;
            string? originalDescription = treatment.Description?.Value;


            Action act = operation switch
            {
                "CorrectName" => () => specialty.CorrectTreatmentName(treatmentId, "!!"),
                "UpdateDescription" => () => specialty.UpdateTreatmentDescription(treatmentId, "!!"),
                "ChangeBaseCost" => () => specialty.ChangeTreatmentBaseCost(treatmentId, -100),
                _ => throw new NotSupportedException()
            };

            // Act
            // Assert
            Assert.ThrowsAny<DomainException>(act);
    

            // no partial mutation
            Treatment unchanged = specialty.Treatments.Single();

            Assert.Equal(originalName, unchanged.Name.Value);
            Assert.Equal(originalCost, unchanged.BaseCost);
            Assert.Equal(originalDescription, unchanged.Description?.Value);
        }
    } 
}