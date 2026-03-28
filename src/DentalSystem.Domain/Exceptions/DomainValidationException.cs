namespace DentalSystem.Domain.Exceptions
{
    public class DomainValidationException : DomainException
    {
        public DomainValidationException(string message) : base(message)
        {
        }
    }
}
