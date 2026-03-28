using DentalSystem.Domain.Exceptions;
using DentalSystem.Domain.ValueObjects.Specialty;

namespace DentalSystem.Domain.Tests.ValueObjectsTests
{
    public sealed class MoneyTests
    {
        /* happy path */

        [Theory]
        [InlineData(1000, "ARS")]
        [InlineData(0, "ARS")]
        [InlineData(500.50, "USD")]
        // Paso un 'double' porque xUnit no soporta 'decimal' en InlineData, 
        // pero lo casteo a (decimal) en los parámetros.
        public void Create_ValidAmount_ShouldSucceed(double amountParam, string currency)
        {
            // Arrange
            decimal amount = (decimal)amountParam;

            // Act
            Money money = new(amount, currency);

            // Assert
            Assert.Equal(amount, money.Amount);
            Assert.Equal(currency, money.Currency);
        }


        /* Error path */
        [Theory]
        [InlineData(-1, "ARS")]     // Monto negativo
        [InlineData(1000, null)]    // Moneda nula
        [InlineData(1000, "")]      // Moneda vacía
        [InlineData(1000, "   ")]   // Moneda con espacios
        public void Create_InvalidArguments_ShouldThrowDomainValidationException(double amountParam, string? currency)
        {
            // Arrange
            decimal amount = (decimal)amountParam;

            // Act AND Assert
            var exception = Assert.Throws<DomainValidationException>(() => new Money(amount, currency!));

            // asegurarse qu eel error no sea nulo
            Assert.NotNull(exception);
        }
    }
}
