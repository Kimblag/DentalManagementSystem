namespace DentalSystem.Domain.Exceptions
{
    public class InvalidSpecialtyDescriptionException : Exception
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
