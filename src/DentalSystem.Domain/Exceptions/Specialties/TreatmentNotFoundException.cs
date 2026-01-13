namespace DentalSystem.Domain.Exceptions.Specialties
{
    public class TreatmentNotFoundException : DomainException
    {

        public TreatmentNotFoundException()
        {
            
        }


        public TreatmentNotFoundException(string message)
            : base(message)
        {
            
        }


        public TreatmentNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
            
        }
    }
}
