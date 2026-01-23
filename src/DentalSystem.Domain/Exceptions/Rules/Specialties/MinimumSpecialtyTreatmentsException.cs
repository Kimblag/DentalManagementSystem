namespace DentalSystem.Domain.Exceptions.Rules.Specialties
{
    public sealed class MinimumSpecialtyTreatmentsException : BusinessRuleViolationException
    {
        private const string DefaultMessage =
            "A specialty must have at least one active treatment.";


        public MinimumSpecialtyTreatmentsException()
            : base(DefaultMessage) { }
    }
}
