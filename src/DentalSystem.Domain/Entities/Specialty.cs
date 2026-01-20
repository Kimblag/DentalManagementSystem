using DentalSystem.Domain.Common;
using DentalSystem.Domain.Exceptions.Rules.Specialties;
using DentalSystem.Domain.ValueObjects;

namespace DentalSystem.Domain.Entities
{
    /// <summary>
    /// Aggregate Root representing a medical specialty.
    /// </summary>
    public class Specialty : AggregateRoot
    {
        public Guid SpecialtyId { get; private set; } // Domain identity
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
        private Specialty() { }

        /// <summary>
        /// Creates a new Specialty aggregate.
        /// </summary>
        public Specialty(
            string name,
            IEnumerable<(string name, decimal baseCost, string? description)> treatments,
            string? description)
        {
            if (treatments is null || !treatments.Any())
                throw new EmptyTreatmentListException();

            SpecialtyId = Guid.NewGuid();
            Name = new Name(name);
            Description = description is null ? null : new Description(description);

            foreach (var t in treatments)
            {
                AddTreatmentInternal(t.name, t.baseCost, t.description);
            }
        }


        //* BEHAVIOR *//

        public void CorrectName(string rawName)
        {
            EnsureActive();

            var newName = new Name(rawName);

            if (Name.Equals(newName))
                return;

            Name = newName;
            
        }

        public void UpdateDescription(string? rawDescription)
        {
            EnsureActive();
            Description = rawDescription is null ? null : new Description(rawDescription);
            
        }

        public void Deactivate()
        {
            Status = Status.Deactivate();
            _treatments.ForEach(t => t.Deactivate());
            
        }

        public void Reactivate()
        {
            Status = Status.Reactivate();
            _treatments.ForEach(t => t.Reactivate());
            
        }


        //* Treatment orchestration *//

        public void AddTreatment(string name, decimal baseCost, string? description)
        {
            EnsureActive();
            AddTreatmentInternal(name, baseCost, description);
            
        }

        public void CorrectTreatmentName(Guid treatmentId, string rawName)
        {
            var treatment = GetActiveTreatmentForModification(treatmentId);

            var previousName = treatment.Name;

            if (_treatments.Any(t => (t.Name.Value.Equals(rawName.Trim(), StringComparison.OrdinalIgnoreCase) 
            && !t.Name.Value.Equals(rawName.Trim(), StringComparison.OrdinalIgnoreCase) )))
                throw new DuplicateTreatmentNameException();

            treatment.CorrectName(rawName);

           
                
        }

        public void ChangeTreatmentBaseCost(Guid treatmentId, decimal newBaseCost)
        {
            var treatment = GetActiveTreatmentForModification(treatmentId);

            var previousCost = treatment.BaseCost;

            treatment.ChangeBaseCost(newBaseCost);
                
        }

        public void UpdateTreatmentDescription(Guid treatmentId, string? rawDescription)
        {
            var treatment = GetActiveTreatmentForModification(treatmentId);

            var previousDescription = treatment.Description;

            treatment.UpdateDescription(rawDescription);

      
                
        }

        public void DeactivateTreatment(Guid treatmentId)
        {
            EnsureActive();

            Treatment treatment = GetActiveTreatmentForDeactivation(treatmentId);

            if (_treatments.Count(t => t.Status.IsActive) == 1)
                throw new MinimumSpecialtyTreatmentsException();

            treatment.Deactivate();
            
        }

        public void ReactivateTreatment(Guid treatmentId)
        {
            EnsureActive();

            Treatment treatment = GetInactiveTreatmentForReactivation(treatmentId);
            treatment.Reactivate();
            
        }


        //* Internals *//
        private void AddTreatmentInternal(string name, decimal baseCost, string? description)
        {
            if (_treatments.Any(t => t.Name.Value.Equals(name.Trim(), StringComparison.OrdinalIgnoreCase)))
                throw new DuplicateTreatmentNameException();

            _treatments.Add(new Treatment(name, baseCost, description));
        }

        // for operations that not change status
        private Treatment GetActiveTreatmentForModification(Guid treatmentId)
        {
            EnsureActive();

            var treatment = _treatments.FirstOrDefault(t => t.TreatmentId == treatmentId)
                ?? throw new TreatmentNotFoundException();

            if (treatment.Status.IsInactive)
                throw new InvalidTreatmentStateException();

            return treatment;
        }

        // for operations that change status
        private Treatment GetActiveTreatmentForDeactivation(Guid treatmentId)
        {
            EnsureActive();

            var treatment = _treatments.FirstOrDefault(t => t.TreatmentId == treatmentId)
                ?? throw new TreatmentNotFoundException();

            if (treatment.Status.IsInactive)
                throw new InvalidStatusTransitionException();

            return treatment;
        }

        private Treatment GetInactiveTreatmentForReactivation(Guid treatmentId)
        {
            EnsureActive();

            var treatment = _treatments.FirstOrDefault(t => t.TreatmentId == treatmentId)
                ?? throw new TreatmentNotFoundException();

            if (treatment.Status.IsActive)
                throw new InvalidStatusTransitionException();

            return treatment;
        }

        private void EnsureActive()
        {
            if (Status.IsInactive)
                throw new InvalidSpecialtyStateException();
        }
    }
}
