using DentalSystem.Domain.Exceptions.Rules.Specialties;
using DentalSystem.Domain.ValueObjects;

namespace DentalSystem.Domain.Entities
{
    /// <summary>
    /// Represents a clinical treatment within a Specialty aggregate.
    /// This entity cannot exist outside its aggregate root.
    /// </summary>
    public class Treatment
    {
        // Domain identity
        public Guid TreatmentId { get; private set; }
        public Name Name { get; private set; } = null!;
        public Description? Description { get; private set; } = null;
        public decimal BaseCost { get; private set; } = 0;
        public LifecycleStatus Status { get; private set; } = LifecycleStatus.Active();

        /// <summary>
        /// Constructor required by EF.
        /// </summary>
        private Treatment()
        {
            // Persistence constructor
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Treatment"/> entity.
        /// </summary>
        /// <param name="name">The domain name value object for the treatment.</param>
        /// <param name="baseCost">The initial monetary cost. Must be greater than or equal to zero.</param>
        /// <param name="description">An optional detailed description of the procedure.</param>
       
        internal Treatment(string name, decimal baseCost, string? description)
        {
            if (baseCost < 0)
                throw new InvalidTreatmentCostException();

            TreatmentId = Guid.NewGuid();
            Name = new Name(name);
            Description = description is null ? null : new Description(description);
            BaseCost = baseCost;
            Status = LifecycleStatus.Active();
        }


        //* Behavior *//
        internal void CorrectName(string rawName)
        {
            EnsureActive();

            var newName = new Name(rawName);

            if (Name.Equals(newName))
                return;

            Name = newName;
        }


        internal void ChangeBaseCost(decimal newBaseCost)
        {
            EnsureActive();

            if (newBaseCost < 0)
                throw new InvalidTreatmentCostException();

            if (BaseCost == newBaseCost)
                return;

            BaseCost = newBaseCost;
        }

        internal void UpdateDescription(string? rawDescription)
        {
            EnsureActive();
            Description = rawDescription is null ? null : new Description(rawDescription);
        }

        internal void Deactivate()
        {
            Status = Status.Deactivate();
        }

        internal void Reactivate()
        {
            Status = Status.Reactivate();
        }

        private void EnsureActive()
        {
            if (Status.IsInactive)
                throw new InvalidTreatmentStateException();
        }

    }
}
