namespace DentalSystem.Domain.Exceptions.Specialties
{
    public class MinimumSpecialtyTreatmentsException : DomainException
    {
        public MinimumSpecialtyTreatmentsException()
        {
            
        }


        public MinimumSpecialtyTreatmentsException(string message)
            : base(message)
        {
            
        }


        public MinimumSpecialtyTreatmentsException(string message, Exception inner)
            : base(message, inner)
        {
            
        }
    }
}
