namespace DentalSystem.Domain.Exceptions.Specialties
{
    public class TreatmentActivationNotAllowedException : Exception
    {
        public TreatmentActivationNotAllowedException()
        {
            
        }


        public TreatmentActivationNotAllowedException(string message)
            : base(message)
        {
            
        }


        public TreatmentActivationNotAllowedException(string message, Exception inner)
            : base(message, inner)
        {
            
        }
    }
}
