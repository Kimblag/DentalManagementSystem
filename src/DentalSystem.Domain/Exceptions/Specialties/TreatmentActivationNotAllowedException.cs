namespace DentalSystem.Domain.Exceptions.Specialties
{
    public class TreatmentActivationNotAllowedException : DomainException
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
