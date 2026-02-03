namespace DentalSystem.Domain.Exceptions.ValueObjects
{
    public class InvalidMoneyException : ValueObjectException
    {
        public InvalidMoneyException(string message) : base(message)
        {
        }
    }
}
