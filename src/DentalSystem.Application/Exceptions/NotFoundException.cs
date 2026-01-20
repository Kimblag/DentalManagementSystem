namespace DentalSystem.Application.Exceptions
{
    public sealed class NotFoundException : ApplicationLayerException
    {
        public string ResourceName { get; }

        public NotFoundException()
        {
            ResourceName = string.Empty;
        }


        public NotFoundException(string resourceName, string message)
            : base(message)
        {
            ResourceName = resourceName;
        }
        

        public NotFoundException(string resourceName, string message, Exception inner)
            : base(message, inner)
        {
            ResourceName = resourceName;
        }
    }
}
