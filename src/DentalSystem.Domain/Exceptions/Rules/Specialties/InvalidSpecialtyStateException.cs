namespace DentalSystem.Domain.Exceptions.Rules.Specialties
{
    public sealed class InvalidSpecialtyStateException : DomainException
    {
        private const string DefaultMessage =
            "The specialty is in an invalid state for this operation.";

        public InvalidSpecialtyStateException()
            : base(DefaultMessage) { }
    }
}
