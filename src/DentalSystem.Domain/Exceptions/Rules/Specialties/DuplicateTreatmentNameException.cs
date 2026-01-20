namespace DentalSystem.Domain.Exceptions.Rules.Specialties
{
    public sealed class DuplicateTreatmentNameException : DomainException
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
