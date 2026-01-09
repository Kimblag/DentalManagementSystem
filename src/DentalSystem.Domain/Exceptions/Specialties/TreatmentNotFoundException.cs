namespace DentalSystem.Domain.Exceptions.Specialties
{
    public class TreatmentNotFoundException : Exception
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
