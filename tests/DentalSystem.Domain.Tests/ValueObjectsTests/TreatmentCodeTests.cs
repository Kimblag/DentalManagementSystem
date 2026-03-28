using DentalSystem.Domain.Exceptions;
using DentalSystem.Domain.ValueObjects.Specialty;

namespace DentalSystem.Domain.Tests.ValueObjectsTests
{
    public sealed class TreatmentCodeTests
    {
        /* Happy path*/

        [Theory]
        [InlineData("01.01")]
        [InlineData("99.99")]
        [InlineData("  05.12  ")]
        public void Create_ValidCode_ShouldSucceed(string value)
        {
            // Act
            TreatmentCode code = new(value);

            // Assert
            Assert.Equal(value.Trim(), code.Value);
        }


        /* Error path*/

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Create_NullOrWhiteSpace_ShouldThrowDomainValidationException(string? value)
        {
            // Act and Assert
            var exception = Assert.Throws<DomainValidationException>(() => new TreatmentCode(value!));

            Assert.Equal("The treatment code is mandatory.", exception.Message);
        }

        [Theory]
        [InlineData("@@@@")]
        [InlineData("01-01")]
        [InlineData("1.01")]
        [InlineData("01.1")]
        [InlineData("001.01")]
        [InlineData("AA.BB")]
        public void Create_InvalidFormat_ShouldThrowDomainValidationException(string value)
        {
            // Act and Assert
            var exception = Assert.Throws<DomainValidationException>(() => new TreatmentCode(value));

            Assert.Equal("The treatment code is invalid.", exception.Message);
        }
    }
}
