namespace DentalSystem.Domain.Exceptions.Rules.Specialties
{
    public sealed class InvalidStatusTransitionException : DomainException
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
