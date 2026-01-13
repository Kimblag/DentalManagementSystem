using DentalSystem.Domain.Enums;
using DentalSystem.Domain.Exceptions.Specialties;
using DentalSystem.Domain.ValueObjects;
using System.Text.RegularExpressions;

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
        public EntityStatus Status { get; private set; }


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
            Status = EntityStatus.Active;
        }

        internal void Deactivate()
        {
            if (Status == EntityStatus.Inactive)
                throw new TreatmentAlreadyInactiveException();

            Status = EntityStatus.Inactive;
        }


        internal void Reactivate()
        {
            if (Status == EntityStatus.Active)
            {
                throw new TreatmentAlreadyActiveException();
            }
            Status = EntityStatus.Active;
        }

        internal void CorrectName(string correctedName)
        {
            // is inactive
            if (Status == EntityStatus.Inactive)
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
