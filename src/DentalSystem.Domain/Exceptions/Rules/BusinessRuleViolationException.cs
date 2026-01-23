namespace DentalSystem.Domain.Exceptions.Rules
{
    public abstract class BusinessRuleViolationException : DomainException
    {
        protected BusinessRuleViolationException(string message)
            : base (message) { }
    }
}
