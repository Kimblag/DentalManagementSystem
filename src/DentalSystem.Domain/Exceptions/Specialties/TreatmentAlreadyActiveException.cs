namespace DentalSystem.Domain.Exceptions.Specialties
{
    public class TreatmentAlreadyActiveException : DomainException
    {
        private const string defaultMessage = "The treatment is already active.";

        public TreatmentAlreadyActiveException()
        {
            
        }


        public TreatmentAlreadyActiveException(string message = defaultMessage)
            : base(message)
        {
            
        }


        public TreatmentAlreadyActiveException(Exception inner, string message = defaultMessage)
            : base(message, inner)
        {
            
        }
    }
}
