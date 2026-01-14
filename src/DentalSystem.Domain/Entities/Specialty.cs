using DentalSystem.Domain.Exceptions.Specialties;
using DentalSystem.Domain.ValueObjects;

namespace DentalSystem.Domain.Entities
{
    // partial because we need to tell .NET that we need to use a generated regex code
    /// <summary>
    /// Represents a medical specialty that acts as the Aggregate Root for clinical procedures.
    /// </summary>
    /// <remarks>
    /// A specialty orchestrates a collection of <see cref="Treatment"/> entities. 
    /// It enforces domain rules such as ensuring at least one treatment exists 
    /// and preventing duplicate procedure names within its scope.
    /// </remarks>
    public class Specialty
    {
        // Domain identity
        public Guid SpecialtyId { get; private set; }

        // EF persistence Identity
        public int Id { get; private set; }
        public Name Name { get; private set; } = null!;
        public Description? Description { get; private set; } = null;
        public LifecycleStatus Status { get; private set; } = LifecycleStatus.Active(); // active by default
        private readonly List<Treatment> _treatments = [];

        // This '=>' acts like a live mirror.It does not store data itself;
        // it simply reflects whatever is currently inside the private '_treatments' list.
        // It provides a 'glass window' that allows external users to see the treatments 
        // without being able to touch or modify the actual collection.
        // while allowing them to iterate and read the data.
        public IReadOnlyCollection<Treatment> Treatments => _treatments.AsReadOnly();


        /// <summary>
        /// Required by Entity Framework or other mappers. 
        /// This constructor ensures the entity can be reconstituted from persistence.
        /// </summary>
        private Specialty()
        {
            // Persistence-only constructor
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Specialty"/> aggregate with its required treatments.
        /// </summary>
        /// <param name="name">The domain name for the specialty.</param>
        /// <param name="treatments">The initial collection of treatments. At least one treatment is required to maintain domain integrity.</param>
        /// <param name="description">An optional clinical description of the specialty.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="name"/> is null.</exception>
        /// <exception cref="EmptyTreatmentListException">Thrown when <paramref name="treatments"/> is empty, violating the mandatory relationship rule.</exception>
        /// <exception cref="DuplicateTreatmentNameException">Thrown when two or more treatments in the initial list share the same name (case-insensitive).</exception>
        public Specialty(Name name, IEnumerable<Treatment> treatments, Description? description)
        {
            // Validate name
            // not null names and match pattern
            ArgumentNullException.ThrowIfNull(name);

            // Validate treatment list is not empty
            if (!treatments.Any())
            {
                throw new EmptyTreatmentListException();
            }


            // Validate treatments uniqueness in the list
            HashSet<string> seenNames = new(StringComparer.OrdinalIgnoreCase);
            foreach (Treatment treatment in treatments)
            {
                // hashset.Add would return false if there is a duplicate
                if (!seenNames.Add(treatment.Name))
                {
                    throw new DuplicateTreatmentNameException($"The treatment '{treatment.Name}' is duplicated.");
                }
            }

            // Create instance
            SpecialtyId = Guid.NewGuid();
            Name = name;
            Description = description;
            _treatments.AddRange(treatments);
        }

        /// <summary>
        /// Corrects the specialty's name to fix typographical errors.
        /// </summary>
        /// <param name="correctedName">The new name string to be validated and assigned.</param>
        /// <exception cref="InvalidSpecialtyStateException">Thrown when trying to modify an inactive specialty.</exception>
        /// <exception cref="InvalidNameException">Thrown when the <paramref name="correctedName"/> format is invalid.</exception>
        public void CorrectName(string correctedName)
        {
            // If specialty is inactive
            if (Status.IsInactive)
                throw new InvalidSpecialtyStateException();

            var newName = new Name(correctedName);
            if (Name.Equals(newName))
                return;

            //Apply correction
            Name = newName;
        }

        /// <summary>
        /// Updates or clears the clinical description of the specialty.
        /// </summary>
        /// <param name="description">The new description value object, or null to remove the current one.</param>
        /// <exception cref="InvalidSpecialtyStateException">Thrown when trying to modify an inactive specialty.</exception>
        public void UpdateDescription(Description? description)
        {
            // Specialty is not active
            if (Status.IsInactive)
            {
                throw new InvalidSpecialtyStateException();
            }

            // change or clear
            Description = description;
        }


        /// <summary>
        /// Reactivates the specialty and all its associated treatments.
        /// </summary>
        /// <remarks>
        /// This operation propagates the active state to the entire treatment collection, 
        /// making them available for clinical use again.
        /// </remarks>
        /// <exception cref="InvalidStatusTransitionException">Thrown when the specialty is already active.</exception>
        public void Reactivate()
        {
            // reactivate treatments
            _treatments
                .ForEach(t => t.Reactivate());

            Status = Status.Reactivate();
        }


        /// <summary>
        /// Deactivates the specialty and all its associated treatments.
        /// </summary>
        /// <remarks>
        /// This operation performs a cascade deactivation. Once inactive, the specialty 
        /// and its treatments are restricted from most domain modifications.
        /// </remarks>
        /// <exception cref="InvalidStatusTransitionException">Thrown when the specialty is already inactive.</exception>
        public void Deactivate()
        {
            // Set children to inactive
            _treatments
                .ForEach(t => t.Deactivate());
            Status = Status.Deactivate();
        }


        /// <summary>
        /// Adds a new treatment to the specialty's collection.
        /// </summary>
        /// <param name="newTreatment">The treatment entity to be added.</param>
        /// <remarks>
        /// This method ensures the specialty is active and validates that no other 
        /// treatment with the same name already exists in the collection.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="newTreatment"/> is null.</exception>
        /// <exception cref="InvalidSpecialtyStateException">Thrown when attempting to add a treatment to an inactive specialty.</exception>
        /// <exception cref="DuplicateTreatmentNameException">Thrown when a treatment with the same name already exists in this specialty.</exception>
        public void AddTreatment(Treatment newTreatment)
        {
            // if treatment is null
            ArgumentNullException.ThrowIfNull(newTreatment);

            // specialty is not active

            if (Status.IsInactive)
            {
                throw new InvalidSpecialtyStateException();
            }

            // Name already exists
            if (_treatments.Any(t => t.Name.Equals(newTreatment.Name)))
            {
                throw new DuplicateTreatmentNameException();
            }


            _treatments.Add(newTreatment);
        }


        /// <summary>
        /// Updates the clinical and financial details of a specific treatment within the specialty.
        /// </summary>
        /// <param name="treatmentId">The unique domain identifier of the treatment to update.</param>
        /// <param name="treatmentBaseCost">The new base cost, if an update is required.</param>
        /// <param name="treatmentDescription">The new description value object, if an update is required.</param>
        /// <param name="treatmentName">The new name for typographical correction, if required.</param>
        /// <remarks>
        /// This method acts as an orchestrator, ensuring both the specialty and the 
        /// specific treatment are in a valid state for modification.
        /// </remarks>
        /// <exception cref="InvalidSpecialtyStateException">Thrown when the specialty is inactive.</exception>
        /// <exception cref="TreatmentNotFoundException">Thrown when no treatment matches the provided <paramref name="treatmentId"/>.</exception>
        /// <exception cref="InvalidTreatmentStateException">Thrown when the target treatment is inactive.</exception>
        /// <exception cref="InvalidNameException">Thrown when the provided <paramref name="treatmentName"/> is invalid.</exception>
        public void UpdateTreatmentDetails(
            Guid treatmentId,
            decimal? treatmentBaseCost = null, 
            Description? treatmentDescription = null,
            string? treatmentName = null)
        {
            if (Status.IsInactive)
            {
                throw new InvalidSpecialtyStateException();
            }

            // Check if treatment exists
            Treatment foundTreatment = _treatments
               .FirstOrDefault(t => t.TreatmentId == treatmentId)
               ?? throw new TreatmentNotFoundException(
                   $"Treatment with ID {treatmentId} was not found in this specialty.");

            // When treatment is inactive
            if (foundTreatment.Status.IsInactive)
            {
                throw new InvalidTreatmentStateException();
            }

            // check if treatmentName has a value
            foundTreatment.CorrectName(treatmentName ?? string.Empty);
            foundTreatment.UpdateDetails(treatmentBaseCost, treatmentDescription);
        }


        /// <summary>
        /// Deactivates a specific treatment within the specialty.
        /// </summary>
        /// <param name="treatmentId">The unique domain identifier of the treatment to deactivate.</param>
        /// <remarks>
        /// This method enforces the business rule that a specialty must maintain at least 
        /// one active treatment. Deactivation will fail if the target is the last active procedure.
        /// </remarks>
        /// <exception cref="InvalidSpecialtyStateException">Thrown when the specialty itself is inactive.</exception>
        /// <exception cref="TreatmentNotFoundException">Thrown when no treatment matches the provided <paramref name="treatmentId"/>.</exception>
        /// <exception cref="TreatmentAlreadyInactiveException">Thrown when the target treatment is already in an inactive state.</exception>
        /// <exception cref="MinimumSpecialtyTreatmentsException">Thrown when attempting to deactivate the last remaining active treatment.</exception>
        public void DeactivateTreatment(Guid treatmentId)
        {
            // When specialty is not active
            if (Status.IsInactive)
            {
                throw new InvalidSpecialtyStateException();
            }

            // treatment found
            Treatment foundTreatment = _treatments
               .FirstOrDefault(t => t.TreatmentId == treatmentId)
               ?? throw new TreatmentNotFoundException(
                   $"Treatment with ID {treatmentId} was not found in this specialty.");

            // When treatment is already inactive
            if (foundTreatment.Status.IsInactive)
            {
                throw new TreatmentAlreadyInactiveException();
            }

            // When is the last active treatment
            if (_treatments.Count(t => t.Status.IsActive) == 1)
            {
                throw new MinimumSpecialtyTreatmentsException("A specialty can't have less than one treatment.");
            }

            // remove
            foundTreatment.Deactivate();
        }

    }
}
