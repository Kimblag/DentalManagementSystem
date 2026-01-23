namespace DentalSystem.Domain.Exceptions.ValueObjects
{
    public abstract class ValueObjectException : DomainException
    {
        protected ValueObjectException(string message)
            : base(message) { }
    }
}
