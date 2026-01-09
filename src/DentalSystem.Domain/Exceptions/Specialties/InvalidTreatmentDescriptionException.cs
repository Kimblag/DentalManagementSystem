namespace DentalSystem.Domain.Exceptions.Specialties
{
    public class InvalidTreatmentDescriptionException : Exception
    {
        public InvalidTreatmentDescriptionException()
        {
            
        }

        public InvalidTreatmentDescriptionException(string message)
            : base(message)
        {
            
        }


        public InvalidTreatmentDescriptionException(string message, Exception inner)
            : base(message, inner)
        {
            
        }
    }
}
