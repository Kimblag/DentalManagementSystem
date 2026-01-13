namespace DentalSystem.Domain.Exceptions.ValueObjects
{
    public class InvalidNameException : DomainException
    {
        private const string defaultMessage = "The provided name is invalid.";

        public InvalidNameException()
        {
            
        }


        public InvalidNameException(string? message = null)
            : base(message ?? defaultMessage)
        {
            
        }


        public InvalidNameException(Exception inner, string? message = null)
            : base(message ?? defaultMessage, inner)
        {
            
        }
    }
}
