namespace DentalSystem.Domain.Exceptions.Specialties
{
    public class InvalidTreatmentNameException : Exception
    {
        public InvalidTreatmentNameException()
        {
            
        }


        public InvalidTreatmentNameException(string message)
            : base(message)
        {
            
        }


        public InvalidTreatmentNameException(string message, Exception inner)
            : base(message, inner)
        {
            
        }
    }
}
