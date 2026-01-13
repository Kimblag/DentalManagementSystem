using DentalSystem.Domain.Entities;
using DentalSystem.Domain.Exceptions;
using DentalSystem.Domain.ValueObjects;

namespace DentalSystem.Domain.Tests.ValueObjectsTests.Specialties
{
    public class Name_CreateShould
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("or")]
        public void Create_WhenNameIsInvalid_ShouldThrowInvalidNameException(string? invalidName)
        {
            // Arrange
            string treatmentDescription = "Metal or ceramic devices to straighten teeth.";
            decimal treatmentBaseCost = 10.0m;

            // Act and Assert
            Assert.ThrowsAny<DomainException>(() =>
            {
                new Treatment(new Name(invalidName!), treatmentBaseCost, treatmentDescription);
            });
        }
    }
}
