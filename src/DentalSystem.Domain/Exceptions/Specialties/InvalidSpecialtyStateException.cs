namespace DentalSystem.Domain.Exceptions.Specialties
{
    public class InvalidSpecialtyStateException : DomainException
    {

        public InvalidSpecialtyStateException()
        {
            
        }


        public InvalidSpecialtyStateException(string message)
            : base(message)
        {
            
        }


        public InvalidSpecialtyStateException(string message, Exception inner)
            : base(message, inner)
        {
            
        }
    }
}
