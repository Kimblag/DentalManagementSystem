namespace DentalSystem.Domain.Exceptions.Specialties
{
    public class InvalidSpecialtyDescriptionException : DomainException
    {

        public InvalidSpecialtyDescriptionException()
        {
            
        }


        public InvalidSpecialtyDescriptionException(string message)
            : base(message)
        {
            
        }


        public InvalidSpecialtyDescriptionException(string message, Exception inner)
            : base(message, inner)
        {
            
        }
    }
}
