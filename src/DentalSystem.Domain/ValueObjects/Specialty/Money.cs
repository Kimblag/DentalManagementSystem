using DentalSystem.Domain.Exceptions;

namespace DentalSystem.Domain.ValueObjects.Specialty
{
    public sealed record Money
    {
        public readonly decimal Amount;
        public readonly string Currency;


        public Money(decimal amount, string currency)
        {
            // amount >= 0
            if (amount < 0)
            {
                throw new DomainValidationException("Amount must be a positive value.");
            }

            if (string.IsNullOrWhiteSpace(currency))
            {
                throw new DomainValidationException("Currency is required.");
            }

            if (currency.Length != 3 || !currency.All(char.IsLetter))
                throw new DomainValidationException("Currency must contains 3 letter character.");

            Amount = amount;
            Currency = currency.ToUpper();
        }
    }
}
