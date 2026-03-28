using DentalSystem.Domain.Exceptions;
using DentalSystem.Domain.ValueObjects;

namespace DentalSystem.Domain.Tests.ValueObjectsTests
{
    public sealed class NameTests
    {

        /* Happy path */

        [Theory]
        [InlineData("Orthodontics")]
        [InlineData("Cleaning")]
        [InlineData("    Composite Simple.   ")]
        [InlineData("X-Ray")]
        public void Create_ValidName_ShouldSucceed(string value)
        {
            // Act
            Name name = new(value);

            // Assert
            Assert.Equal(value.Trim(), name.Value);
        }


        /* Error path */
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Create_NullOrWhiteSpace_ShouldThrowDomainValidationException(string? value)
        {
            // Act and Assert
            var exception = Assert.Throws<DomainValidationException>(() => new Name(value!));

            Assert.Equal("Name is mandatory.", exception.Message);
        }

        [Theory]
        [InlineData("A")]
        [InlineData("@@@@@")] 
        [InlineData("Ortodoncia_General")]
        public void Create_InvalidFormat_ShouldThrowDomainValidationException(string value)
        {
            // Act and Assert
            var exception = Assert.Throws<DomainValidationException>(() => new Name(value));

            Assert.Equal("The name's format is invalid.", exception.Message);
        }

        [Fact]
        public void Create_TooLongName_ShouldThrowDomainValidationException()
        {
            string tooLongName = new('A', 105);

            // Act and Assert
            var exception = Assert.Throws<DomainValidationException>(() => new Name(tooLongName));
            Assert.Equal("The name's format is invalid.", exception.Message);
        }
    }
}
