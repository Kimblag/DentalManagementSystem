namespace DentalSystem.Domain.Exceptions.ValueObjects
{
    public abstract class ValueObjectException : DomainException
    {
        protected ValueObjectException()
        {
            
        }


        protected ValueObjectException(string message)
            : base(message)
        {
            
        }


        protected ValueObjectException(string message, Exception inner)
            : base(message, inner)
        {
            
        }
    }
}
