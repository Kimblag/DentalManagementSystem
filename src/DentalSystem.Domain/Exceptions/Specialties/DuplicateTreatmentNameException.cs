namespace DentalSystem.Domain.Exceptions.Specialties
{
    public class DuplicateTreatmentNameException : Exception
    {

        public DuplicateTreatmentNameException()
        {
            
        }


        public DuplicateTreatmentNameException(string message)
            : base(message)
        {
            
        }



        public DuplicateTreatmentNameException(string message, Exception inner)
            : base(message, inner)
        {
            
        }
    }
}
