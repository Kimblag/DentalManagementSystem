using DentalSystem.Domain.Exceptions.ValueObjects;

namespace DentalSystem.Domain.ValueObjects
{
    public sealed class Money
    {
        public readonly decimal Amount;
        public readonly string Currency;


        public Money(decimal amount, string currency)
        {
            // amount >= 0
            if (amount < 0)
            {
                throw new InvalidMoneyException("Amount must be a positive value.");
            }

            if (string.IsNullOrWhiteSpace(currency))
            {
                throw new InvalidMoneyException("Currency is required.");
            }

            if (currency.Length != 3 || !currency.All(char.IsLetter))
                throw new InvalidMoneyException("Currency must contains 3 letter character.");

            Amount = amount;
            Currency = currency.ToUpper();
        }

        public override bool Equals(object? obj)
        {
            return obj is Money money &&
                   Amount == money.Amount &&
                   Currency == money.Currency;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Amount, Currency);
        }
    }
}
