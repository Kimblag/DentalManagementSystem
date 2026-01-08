namespace DentalSystem.Domain.Exceptions
{
    public class InvalidTreatmentCostException : Exception
    {

        public InvalidTreatmentCostException()
        {
            
        }


        public InvalidTreatmentCostException(string message)
            : base(message)
        {
            
        }


        public InvalidTreatmentCostException(string message, Exception inner)
            : base(message, inner)
        {
            
        }
    }
}
