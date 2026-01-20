namespace DentalSystem.Domain.Exceptions.Rules
{
    public abstract class BusinessRuleViolationException : DomainException
    {

        protected BusinessRuleViolationException()
        {
            
        }

        protected BusinessRuleViolationException(string message)
            : base (message)
        {
            
        }

        protected BusinessRuleViolationException(string message, Exception inner)
            : base(message, inner)
        {
            
        }
    }
}
