namespace DentalSystem.Domain.Exceptions.ValueObjects
{
    public sealed class InvalidDescriptionException : ValueObjectException
    {
        private const string DefaultMessage = "The provided description is invalid.";

        public InvalidDescriptionException()
            : base(DefaultMessage) { }
    }
}
