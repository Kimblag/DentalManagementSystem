namespace DentalSystem.Domain.Exceptions.Rules.Specialties
{
    public sealed class EmptyTreatmentListException : BusinessRuleViolationException
    {
        private const string DefaultMessage =
            "A specialty must have at least one treatment.";

        public EmptyTreatmentListException()
            : base(DefaultMessage) { }
    }
}
