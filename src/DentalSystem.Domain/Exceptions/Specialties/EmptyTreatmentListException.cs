namespace DentalSystem.Domain.Exceptions.Specialties
{
    public class EmptyTreatmentListException : DomainException
    {
        public EmptyTreatmentListException()
        {
            
        }


        public EmptyTreatmentListException(string message)
            : base(message)
        {
            
        }


        public EmptyTreatmentListException(string message, Exception inner)
            : base(message, inner)
        {
            
        }
    }
}
