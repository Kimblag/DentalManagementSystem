namespace DentalSystem.Domain.Exceptions.Rules.Specialties
{
    public sealed class DuplicateTreatmentNameException : BusinessRuleViolationException
    {
        private const string DefaultMessage =
           "A treatment with the same name already exists in the specialty.";

        public DuplicateTreatmentNameException()
            : base(DefaultMessage) { }
    }
}
