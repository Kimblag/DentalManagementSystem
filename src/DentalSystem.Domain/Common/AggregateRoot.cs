namespace DentalSystem.Domain.Common
{
    public abstract class AggregateRoot
    {
        public int Version { get; private set; } = 0;

        protected void MarkAsModified()
        {
            Version++;
        }
    }
}
