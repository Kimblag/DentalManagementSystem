namespace DentalSystem.Domain.Exceptions.Specialties
{
    public class TreatmentAlreadyInactiveException : DomainException
    {
        public TreatmentAlreadyInactiveException()
        {
            
        }


        public TreatmentAlreadyInactiveException(string message)
            : base(message)
        {
            
        }

        public TreatmentAlreadyInactiveException(string message, Exception inner)
            : base(message, inner)
        {
            
        }

    }
}
