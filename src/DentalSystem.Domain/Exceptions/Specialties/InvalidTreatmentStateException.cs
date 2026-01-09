namespace DentalSystem.Domain.Exceptions.Specialties
{
    public class InvalidTreatmentStateException : Exception
    {
        public InvalidTreatmentStateException()
        {
            
        }


        public InvalidTreatmentStateException(string message)
            : base(message)
        {
            
        }

        public InvalidTreatmentStateException(string message, Exception inner)
            : base(message, inner)
        {
            
        }
    }
}
