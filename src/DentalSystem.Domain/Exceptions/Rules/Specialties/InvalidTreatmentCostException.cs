namespace DentalSystem.Domain.Exceptions.Rules.Specialties
{
    public sealed class InvalidTreatmentCostException : DomainException
    {
        private const string defaultMessage = "Cost cannot be negative";

        public InvalidTreatmentCostException()
        {
            
        }


        public InvalidTreatmentCostException(string message = defaultMessage)
            : base(message)
        {
            
        }


        public InvalidTreatmentCostException(Exception inner, string message = defaultMessage)
            : base(message, inner)
        {
            
        }
    }
}
