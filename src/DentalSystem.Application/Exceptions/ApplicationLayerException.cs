namespace DentalSystem.Application.Exceptions
{
    public abstract class ApplicationLayerException : Exception
    {

        protected ApplicationLayerException()
        {
            
        }

        protected ApplicationLayerException(string message)
            : base(message)
        {
            
        }

        protected ApplicationLayerException(string message, Exception inner)
            : base(message, inner)
        {
            
        }
    }
}
