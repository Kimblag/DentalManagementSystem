using DentalSystem.Domain.Exceptions;
using DentalSystem.Domain.Exceptions.Specialties;
using DentalSystem.Domain.Tests.Builder;
using DentalSystem.Domain.Tests.Helpers;
using DentalSystem.Domain.ValueObjects;

namespace DentalSystem.Domain.Tests.EntitiesTests.Specialties
{
    public class Treatment_CorrectNameShould
    {
        // Happy path
        [Theory]
        [InlineData("Dental Cleaning")]
        [InlineData(" Dental Cleaning")]
        [InlineData(" Dental Cleaning ")]
        public void CorrectName_WhenValidName_ShouldUpdateName(string inputName)
        {
            // Arrange
            var treatment = TreatmentBuilder.CreateValid("Clean");

            // take snapshot
            Guid originalId = treatment.TreatmentId;
            string? originalDescription = treatment.Description?.Value;
            decimal originalBaseCost = treatment.BaseCost;
            LifecycleStatus originalStatus = treatment.Status;

            string expectedName = "Dental Cleaning";

            // Act
            treatment.CorrectName(inputName);

            // Assert
            Assert.Equal(expectedName, treatment.Name);
            treatment.AssertInvariants(
              originalId,
              originalStatus,
              originalBaseCost,
              expectedDescription: originalDescription
            );
        }


        // Error
        [Fact]
        public void CorrectName_WhenTreatmentIsInactive_ShouldThrowInvalidTreatmentStateException()
        {
            // Arrange
            var treatment = TreatmentBuilder.CreateValid("Clean");
            treatment.Deactivate();
            
            // take snapshot
            Guid originalId = treatment.TreatmentId;
            string originalName = treatment.Name;
            string? originalDescription = treatment.Description?.Value;
            decimal originalBaseCost = treatment.BaseCost;
            LifecycleStatus originalStatus = treatment.Status;

            // Act
            // Assert
            Assert.Throws<InvalidTreatmentStateException>(() =>
            {
                treatment.CorrectName("Dental Cleaning");
            });

            treatment.AssertInvariants(
              originalId,
              originalStatus,
              originalBaseCost,
              originalName,
              originalDescription
            );
        }


        [Theory]
        [InlineData("a")]
        [InlineData(" ")]
        [InlineData("")]
        [InlineData("!@#$%")]
        public void CorrectName_WhenNameIsInvalid_ShouldThrowInvalidTreatmentNameException(string invalidName)
        {
            // Arrange
            var treatment = TreatmentBuilder.CreateValid("Braces");

            // take snapshot
            Guid originalId = treatment.TreatmentId;
            string originalName = treatment.Name;
            string? originalDescription = treatment.Description?.Value;
            decimal originalBaseCost = treatment.BaseCost;
            LifecycleStatus originalStatus = treatment.Status;

            // Act
            // Assert
            Assert.ThrowsAny<DomainException>(() =>
            {
                treatment.CorrectName(invalidName);
            });

            treatment.AssertInvariants(
              originalId,
              originalStatus,
              originalBaseCost,
              originalName,
              originalDescription
            );
        }


        [Theory]
        [InlineData("Extraction")]
        [InlineData(" Extraction")]
        [InlineData(" extraction ")]
        public void CorrectName_WhenNameIsIdentical_ShouldNotMutateAnyProperty(string duplicateName)
        {
            // Arrange
            var treatment = TreatmentBuilder.CreateValid("Extraction");

            // take snapshot
            Guid originalId = treatment.TreatmentId;
            string originalName = treatment.Name;
            string? originalDescription = treatment.Description?.Value;
            decimal originalBaseCost = treatment.BaseCost;
            LifecycleStatus originalStatus = treatment.Status;

            // Act
            treatment.CorrectName(duplicateName);

            // Assert
            // Should remain unchanged
            treatment.AssertInvariants(
             originalId,
             originalStatus,
             originalBaseCost,
             originalName,
             originalDescription
           );
        }
    }
}
