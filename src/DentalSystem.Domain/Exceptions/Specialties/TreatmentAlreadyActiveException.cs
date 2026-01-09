namespace DentalSystem.Domain.Exceptions.Specialties
{
    public class TreatmentAlreadyActiveException : Exception
    {
        private const string defaultMessage = "The treatment is already active.";

        public TreatmentAlreadyActiveException()
        {
            
        }


        public TreatmentAlreadyActiveException(string message = defaultMessage)
            : base(message)
        {
            
        }


        public TreatmentAlreadyActiveException(string message = defaultMessage, Exception inner)
            : base(message, inner)
        {
            
        }
    }
}
