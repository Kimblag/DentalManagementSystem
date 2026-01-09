namespace DentalSystem.Domain.Exceptions.Specialties
{
    public class InvalidTreatmentCostException : Exception
    {
        private const string defaultMessage = "Cost cannot be negative";

        public InvalidTreatmentCostException()
        {
            
        }


        public InvalidTreatmentCostException(string message = defaultMessage)
            : base(message)
        {
            
        }


        public InvalidTreatmentCostException(string message = defaultMessage, Exception inner)
            : base(message, inner)
        {
            
        }
    }
}
