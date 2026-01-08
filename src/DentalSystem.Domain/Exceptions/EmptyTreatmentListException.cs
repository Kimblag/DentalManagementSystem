namespace DentalSystem.Domain.Exceptions
{
    public class EmptyTreatmentListException : Exception
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
