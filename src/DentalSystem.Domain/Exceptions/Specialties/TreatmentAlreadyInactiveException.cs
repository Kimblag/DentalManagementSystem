namespace DentalSystem.Domain.Exceptions.Specialties
{
    public class TreatmentAlreadyInactiveException : Exception
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
