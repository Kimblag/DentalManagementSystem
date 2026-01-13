using DentalSystem.Domain.Exceptions;
using DentalSystem.Domain.ValueObjects;

namespace DentalSystem.Domain.Tests.ValueObjectsTests.DescriptionTests
{
    public  class Description_CreateShould
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Create_WhenDescriptionIsEmpty_ShouldBeNull(string? emptyDescription)
        {
            // Act
            var description = new Description(emptyDescription!);

            // Assert
            Assert.Null(description.Value);
        }

        [Theory]
        [InlineData("Basic preventive treatment.")]
        [InlineData("Tratamiento con aparatos metálicos.")]
        public void Create_WhenDescriptionIsValid_ShouldStoreValue(string validDescription)
        {
            // Act
            var description = new Description(validDescription);

            // Assert
            Assert.Equal(validDescription, description.Value);
        }

        [Theory]
        [InlineData("ab")] // too short
        [InlineData("!@#$$%")] // invalid characters regex
        public void Create_WhenDescriptionIsInvalid_ShouldThrowInvalidDescriptionException(string invalidDescription)
        {
            // Act
            // Assert
            Assert.ThrowsAny<DomainException>(() =>
            {
                _ = new Description(invalidDescription);
            });
        }
    }
}
