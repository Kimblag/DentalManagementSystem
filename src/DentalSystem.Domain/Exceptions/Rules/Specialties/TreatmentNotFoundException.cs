namespace DentalSystem.Domain.Exceptions.Rules.Specialties
{
    public sealed class TreatmentNotFoundException : DomainException
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
