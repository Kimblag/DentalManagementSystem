namespace DentalSystem.Domain.Exceptions.Rules.Specialties
{
    public sealed class InvalidTreatmentCostException : DomainException
    {
        private const string DefaultMessage =
            "Treatment base cost cannot be negative.";

        public InvalidTreatmentCostException()
            : base(DefaultMessage) { }
    }
}
