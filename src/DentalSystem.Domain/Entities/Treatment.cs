using DentalSystem.Domain.Enums;
using DentalSystem.Domain.Exceptions.Specialties;
using DentalSystem.Domain.ValueObjects;

namespace DentalSystem.Domain.Entities
{
    public partial class Treatment
    {
        // Domain identity
        public Guid TreatmentId { get; private set; }

        // EF persistence Identity
        public int Id { get; private set; }
        public Name Name { get; private set; } = null!;
        public Description? Description { get; private set; } = null;
        public decimal BaseCost { get; private set; } = 0;
        public LifecycleStatus Status { get; private set; } = new LifecycleStatus();


        private Treatment()
        {
            
        }

        public Treatment(Name name, decimal baseCost, Description? description)
        {
            // Check name
            ArgumentNullException.ThrowIfNull(name);


            // check base cost
            if (baseCost < 0)
            {
                throw new InvalidTreatmentCostException();
            }


            TreatmentId = Guid.NewGuid();
            Name = name;
            Description = description ?? null;
            BaseCost = baseCost;
        }

        internal void Deactivate()
        {
            if (Status.IsInactive)
                throw new TreatmentAlreadyInactiveException();

            Status.Deactivate();
        }


        internal void Reactivate()
        {
            if (Status.IsActive)
            {
                throw new TreatmentAlreadyActiveException();
            }
            Status.Reactivate();
        }

        internal void CorrectName(string correctedName)
        {
            // is inactive
            if (Status.IsInactive)
            {
                throw new InvalidTreatmentStateException();
            }

            var newName = new Name(correctedName);

            if (Name.Equals(newName))
                return;

            Name = newName;
        }

        internal void UpdateDetails(decimal? newBaseCost = null, Description? newDescription = null)
        {
            // if there is a new base cost
            if (newBaseCost.HasValue && newBaseCost < 0)
            {
               throw new InvalidTreatmentCostException();
            }
           
            BaseCost = newBaseCost ?? BaseCost;
            Description = newDescription ?? null;
        }


    }
}
