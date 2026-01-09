namespace DentalSystem.Domain.Exceptions.Specialties
{
    public class InvalidSpecialtyStateException : Exception
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
