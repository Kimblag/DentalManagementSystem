namespace DentalSystem.Domain.Exceptions.Specialties
{
    public class InvalidTreatmentStateException : DomainException
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
