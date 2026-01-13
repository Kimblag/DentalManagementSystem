namespace DentalSystem.Domain.Exceptions.ValueObjects
{
    public class InvalidDescriptionException : DomainException
    {

        private const string defaultMessage = "The provided description is invalid.";
        public InvalidDescriptionException()
        {
            
        }


        public InvalidDescriptionException(string? message = null)
            : base(message ?? defaultMessage)
        {
            
        }


        public InvalidDescriptionException(Exception inner, string? message = null)
            : base(message ?? defaultMessage, inner)
        {
            
        }
    }
}
