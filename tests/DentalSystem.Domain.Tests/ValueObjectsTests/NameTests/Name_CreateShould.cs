using DentalSystem.Domain.Exceptions;
using DentalSystem.Domain.ValueObjects;

namespace DentalSystem.Domain.Tests.ValueObjectsTests.NameTests
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
            Assert.ThrowsAny<DomainException>(() =>
            {
                _ = new Name(invalidName!);
            });
        }


        [Theory]
        [InlineData("Orthodontics")]
        [InlineData("  Orthodontics  ")]
        [InlineData("Endodontics")]
        public void Create_WhenNameIsValid_ShouldTrimAndStoreValue(string validName)
        {
            // Act
            var name = new Name(validName);

            // Assert
            Assert.Equal(validName.Trim(), name.Value);
        }
    }
}
