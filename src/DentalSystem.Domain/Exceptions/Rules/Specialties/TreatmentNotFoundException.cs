namespace DentalSystem.Domain.Exceptions.Rules.Specialties
{
    public sealed class TreatmentNotFoundException : DomainException
    {
        private const string DefaultMessage = "Treatment not found.";

        public TreatmentNotFoundException()
            : base(DefaultMessage) { }
    }
}
