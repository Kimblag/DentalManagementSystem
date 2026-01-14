using DentalSystem.Domain.Exceptions.Specialties;
using DentalSystem.Domain.ValueObjects;

namespace DentalSystem.Domain.Entities
{
    // partial because we need to tell .NET that we need to use a generated regex code
    public partial class Specialty
    {
        // Domain identity
        public Guid SpecialtyId { get; private set; }

        // EF persistence Identity
        public int Id { get; private set; }
        public Name Name { get; private set; } = null!;
        public Description? Description { get; private set; } = null;
        public LifecycleStatus Status { get; private set; } = new LifecycleStatus(); // active by default
        private readonly List<Treatment> _treatments = [];

        // This '=>' acts like a live mirror.It does not store data itself;
        // it simply reflects whatever is currently inside the private '_treatments' list.
        // It provides a 'glass window' that allows external users to see the treatments 
        // without being able to touch or modify the actual collection.
        // while allowing them to iterate and read the data.
        public IReadOnlyCollection<Treatment> Treatments => _treatments.AsReadOnly();


        private Specialty()
        {
            
        }

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
            Description = description ?? null;
            _treatments.AddRange(treatments);
        }


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


        public void UpdateDescription(Description? description)
        {
            // Specialty is not active
            if (Status.IsInactive)
            {
                throw new InvalidSpecialtyStateException();
            }

            // change or clear
            Description = description ?? null;
        }


        public void Reactivate()
        {
            // Is already active
            if (Status.IsActive)
            {
                throw new InvalidStatusTransitionException("The specialty is already active.");
            }

            // reactivate treatments
            foreach (var treatment in _treatments)
            {
                treatment.Reactivate();
            }

            Status.Reactivate();
        }


        public void Deactivate()
        {
            // Check current status
            if (Status.IsInactive)
            {
                throw new InvalidStatusTransitionException();
            }

            // Set children to inactive
            _treatments
                .ForEach(t => t.Deactivate());
            Status.Deactivate();
        }


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
