namespace DentalSystem.Api.Contracts.Specialties
{
    public sealed class CreateSpecialtyRequest
    {
        // init: the property can only be assigned during the object's
        // construction; after that, it is immutable.
        public string Name { get; init; } = string.Empty;
        public string? Description { get; init; }
        public List<CreateTreatmentRequest> Treatments { get; init; } = [];
    }

    public sealed class CreateTreatmentRequest
    {
        public string Name { get; init; } = string.Empty;
        public decimal BaseCost { get; init; }
        public string? Description { get; init; }
    }
}
