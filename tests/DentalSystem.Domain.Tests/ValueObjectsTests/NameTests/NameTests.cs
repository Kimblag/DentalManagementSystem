using DentalSystem.Domain.Exceptions.ValueObjects;
using DentalSystem.Domain.ValueObjects;

namespace DentalSystem.Domain.Tests.ValueObjectsTests.NameTests
{
    public class NameTests
    {
        /* Creation scenario */

        // Happy path
        [Theory]
        [InlineData("Orthodoncy")]
        [InlineData(" Orthodoncy ")]
        public void Create_ValidName_ShouldSucceed(string value)
        {
            // Act
            Name name = new(value);

            // Assert
            Assert.Equal("Orthodoncy", name.Value);
        }



        // Error path

        // Invalid values
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("A")]
        [InlineData("@@@@@")]
        public void Create_InvalidName_ShouldThrowInvalidNameException(string invalidValue)
        {
            // Act
            // Assert
            Assert.Throws<InvalidNameException>(() =>
            {
                Name name = new(invalidValue);
            });
        }


        [Fact]
        public void Create_EqualsValue_ShouldSucceed()
        {
            // Arrange
            Name name1 = new("Orthodoncy");
            Name name2 = new("Orthodoncy");
            // Act
            // Assert
            Assert.True(name1.Equals(name2));
        }
 
        [Fact]
        public void Create_EqualsDifferentValue_ShouldReturnFalse()
        {
            //Arrange
            Name name1 = new("Orthodoncy");
            Name name2 = new("Endodontics");
            // Act
            // Assert
            Assert.False(name1.Equals(name2));
        }

    }
}
