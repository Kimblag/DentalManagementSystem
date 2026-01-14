using DentalSystem.Domain.Exceptions.Specialties;
using DentalSystem.Domain.ValueObjects;

namespace DentalSystem.Domain.Entities
{
    /// <summary>
    /// Represents a specific medical procedure or clinical intervention within a specialized field.
    /// </summary>
    /// <remarks>
    /// This entity is part of the <see cref="Specialty"/> Aggregate. 
    /// A treatment cannot exist independently of its parent specialty, 
    /// ensuring domain consistency within the clinical scope.
    /// </remarks>
    public class Treatment
    {
        // Domain identity
        public Guid TreatmentId { get; private set; }

        // EF persistence Identity
        public int Id { get; private set; }
        public Name Name { get; private set; } = null!;
        public Description? Description { get; private set; } = null;
        public decimal BaseCost { get; private set; } = 0;
        public LifecycleStatus Status { get; private set; } = LifecycleStatus.Active();

        /// <summary>
        /// Required by Entity Framework or other mappers.
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
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="name"/> is null.</exception>
        /// <exception cref="InvalidTreatmentCostException">Thrown when <paramref name="baseCost"/> is negative.</exception>
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
            Description = description;
            BaseCost = baseCost;
        }

        /// <summary>
        /// Deactivates the treatment, making it unavailable for future clinical procedures.
        /// </summary>
        /// <remarks>
        /// This operation is managed by the internal status of the aggregate.
        /// </remarks>
        /// <exception cref="InvalidStatusTransitionException">
        /// Thrown when the treatment is already inactive.
        /// </exception>
        internal void Deactivate()
        {
            Status = Status.Deactivate();
        }

        /// <summary>
        /// Reactivates the treatment, making it available again for clinical use.
        /// </summary>
        /// <remarks>
        /// This operation is managed by the internal status of the aggregate.
        /// </remarks>
        /// <exception cref="InvalidStatusTransitionException">
        /// Thrown when the treatment is already active.
        /// </exception>
        internal void Reactivate()
        {
            Status = Status.Reactivate();
        }


        /// <summary>
        /// Corrects the treatment name due to typographical errors.
        /// </summary>
        /// <remarks>
        /// This method is intended for minor corrections only. Significant changes to the treatment's 
        /// purpose should be handled by creating a new treatment to preserve clinical history.
        /// </remarks>
        /// <param name="correctedName">The new name string to be validated and assigned.</param>
        /// <exception cref="InvalidTreatmentStateException">
        /// Thrown when attempting to modify a treatment that is currently inactive.
        /// </exception>
        /// <exception cref="InvalidNameException">
        /// Thrown when the <paramref name="correctedName"/> does not meet the domain format requirements.
        /// </exception>
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

        /// <summary>
        /// Updates the treatment's clinical details and pricing.
        /// </summary>
        /// <remarks>
        /// This method allows for partial updates. If a parameter is not provided, 
        /// the current value remains unchanged. Providing a null description will clear the existing one.
        /// </remarks>
        /// <param name="newBaseCost">The updated monetary cost. Must be zero or positive.</param>
        /// <param name="newDescription">
        /// The updated <see cref="Description"/> value object. Pass null to remove the current description.
        /// </param>
        /// <exception cref="InvalidTreatmentCostException">
        /// Thrown when <paramref name="newBaseCost"/> is a negative value.
        /// </exception>
        internal void UpdateDetails(decimal? newBaseCost = null, Description? newDescription = null)
        {
            // if there is a new base cost
            if (newBaseCost.HasValue && newBaseCost < 0)
            {
               throw new InvalidTreatmentCostException();
            }
           
            BaseCost = newBaseCost ?? BaseCost;
            Description = newDescription;
        }


    }
}
