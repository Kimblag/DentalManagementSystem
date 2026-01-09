namespace DentalSystem.Domain.Exceptions.Specialties
{
    public class NoChangesDetectedException : Exception
    {
        public NoChangesDetectedException()
        {
            
        }


        public NoChangesDetectedException(string message)
            : base(message)
        {
            
        }

        public NoChangesDetectedException(string message, Exception inner)
            : base(message, inner)
        {
            
        }
    }
}
