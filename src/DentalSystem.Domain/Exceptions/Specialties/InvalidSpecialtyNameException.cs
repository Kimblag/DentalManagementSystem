namespace DentalSystem.Domain.Exceptions.Specialties
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
