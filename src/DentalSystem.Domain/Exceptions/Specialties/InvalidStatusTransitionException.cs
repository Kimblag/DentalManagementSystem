namespace DentalSystem.Domain.Exceptions.Specialties
{
    public class InvalidStatusTransitionException : Exception
    {

        public InvalidStatusTransitionException()
        {
            
        }


        public InvalidStatusTransitionException(string message)
            : base(message)
        {
            
        }


        public InvalidStatusTransitionException(string message, Exception inner)
            : base(message, inner)
        {
            
        }
    }
}
