namespace DentalSystem.Application.Exceptions
{
    public class SpecialtyNotFoundException : Exception
    {
        public SpecialtyNotFoundException()
        {
            
        }


        public SpecialtyNotFoundException(string message)
            : base(message)
        {
            
        }
        

        public SpecialtyNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
            
        }
    }
}
