using DentalSystem.Domain.Exceptions.ValueObjects;
using DentalSystem.Domain.ValueObjects;
using System.Runtime.CompilerServices;

namespace DentalSystem.Domain.Tests.ValueObjectsTests.MoneyTests
{
    public sealed class MoneyTests
    {
        /* happy path */

        // Creation

        // arrange
        public static IEnumerable<object[]> AmountsAndCurrencies =>
        [
            [0m, "ARS"],
            [500m, "USD"],
            [1000m, "CLP"],
        ];

        [Theory]
        [MemberData(nameof(AmountsAndCurrencies))]
        public void Create_ValidAmount_ShouldSucceed(decimal amount, string currency)
        {

            // act
            Money money = new(amount, currency);

            //assert
            Assert.Equal(amount, money.Amount);
            Assert.Equal(currency, money.Currency);
        }

        // Equals by value
        [Fact]
        public void Create_EqualValues_ShouldReturnTrue()
        {
            Money money = new(1000m, "ARS");
            Money moneyCopy = new(1000m, "ARS");

            Assert.True(money.Equals(moneyCopy));
        }


        // Different amount value
        [Fact]
        public void Create_DifferentAmountValues_ShouldReturnFalse()
        {
            Money money = new(1000m, "ARS");
            Money moneyCopy = new(2000m, "ARS");

            Assert.False(money.Equals(moneyCopy));
        }


        // different currency value
        [Fact]
        public void Create_DifferentCurrencyValues_ShouldReturnFalse()
        {
            Money money = new(1000m, "USD");
            Money moneyCopy = new(1000m, "ARS");

            Assert.False(money.Equals(moneyCopy));
        }


        /* error path */

        // invalid amounts: negative, null, empty
        // arrange
        public static IEnumerable<object[]> InvalidAmountsAndCurrencies =>
        [
            [-1m, "ARS"],
            [1000m, null],
            [1000m, ""],
        ];
        [Theory]
        [MemberData(nameof(InvalidAmountsAndCurrencies))]
        public void Create_InvalidAmount_ShouldThrowInvalidAmount(decimal amount, string currency)
        {
            // act and assert
            Assert.Throws<InvalidMoneyException>(() =>  new Money(amount, currency));
        }

    }
}
