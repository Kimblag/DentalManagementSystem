namespace DentalSystem.Domain.Exceptions.Rules.Specialties
{
    public sealed class InvalidTreatmentStateException : DomainException
    {
        private const string DefaultMessage =
            "The treatment is in an invalid state for this operation.";

        public InvalidTreatmentStateException()
            : base(DefaultMessage) { }
    }
}
