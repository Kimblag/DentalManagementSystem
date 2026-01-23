namespace DentalSystem.Domain.Exceptions.Rules.Specialties
{
    public sealed class InvalidStatusTransitionException : DomainException
    {
        private const string DefaultMessage =
        "The requested status transition is not allowed.";

        public InvalidStatusTransitionException()
            : base(DefaultMessage) { }

    }
}
