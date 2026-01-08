namespace DentalSystem.Domain.Exceptions
{
    public class InvalidSpecialtyNameException : Exception
    {

        public InvalidSpecialtyNameException()
        {
            
        }


        public InvalidSpecialtyNameException(string message)
            : base(message)
        {
            
        }


        public InvalidSpecialtyNameException(string message, Exception inner)
            : base(message, inner)
        {
            
        }
    }
}
