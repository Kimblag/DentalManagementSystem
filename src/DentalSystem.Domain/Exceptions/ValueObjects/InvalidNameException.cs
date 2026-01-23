namespace DentalSystem.Domain.Exceptions.ValueObjects
{
    public sealed class InvalidNameException : ValueObjectException
    {
        private const string DefaultMessage = "The provided name is invalid.";

        public InvalidNameException()
            : base(DefaultMessage) { }
    }
}
