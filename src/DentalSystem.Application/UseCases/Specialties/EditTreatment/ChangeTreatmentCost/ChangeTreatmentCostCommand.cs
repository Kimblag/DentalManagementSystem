namespace DentalSystem.Application.UseCases.Specialties.EditTreatment.ChangeTreatmentCost
{
    public sealed class ChangeTreatmentCostCommand
    {
        public Guid SpecialtyId { get; }
        public Guid TreatmentId { get; }
        public decimal BaseCost { get; }

        public ChangeTreatmentCostCommand(Guid specialtyId, Guid treatmentId, decimal baseCost)
        {
            if (specialtyId == Guid.Empty || treatmentId == Guid.Empty)
                throw new ArgumentException("SpecialtyId, TreatmentID and Base Cost cannot be empty.");

            SpecialtyId = specialtyId;
            TreatmentId = treatmentId;
            BaseCost = baseCost;
        }
    }
}
