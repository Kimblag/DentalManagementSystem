using DentalSystem.Domain.Enums;
using DentalSystem.Domain.Exceptions.Specialties;
using System.Text.RegularExpressions;

namespace DentalSystem.Domain.Entities
{
    // partial because we need to tell .NET that we need to use a generated regex code
    public partial class Specialty
    {
        // Domain identity
        public Guid SpecialtyId { get; private set; }

        // EF persistence Identity
        public int Id { get; private set; }
        public string Name { get; private set; } = null!;
        public string Description { get; private set; } = string.Empty;
        public EntityStatus Status { get; private set; }
        private readonly List<Treatment> _treatments = new();

        // This '=>' acts like a live mirror.It does not store data itself;
        // it simply reflects whatever is currently inside the private '_treatments' list.
        // It provides a 'glass window' that allows external users to see the treatments 
        // without being able to touch or modify the actual collection.
        // while allowing them to iterate and read the data.
        public IReadOnlyCollection<Treatment> Treatments => _treatments.AsReadOnly();

        [GeneratedRegex("^[a-zA-Z0-9áéíóúñÁÉÍÓÚÑ][a-zA-Z0-9áéíóúñÁÉÍÓÚÑ\\s-]{3,100}[a-zA-Z0-9áéíóúñÁÉÍÓÚÑ]$")]
        private static partial Regex NamePattern();

        [GeneratedRegex("^[\\w\\sáéíóúñÁÉÍÓÚÑ\\.,;:\\-\\(\\)¿?¡!]{3,500}$")]
        private static partial Regex DescriptionPattern();


        private Specialty()
        {
            
        }

        public Specialty(string name, IEnumerable<Treatment> treatments, string? description)
        {
            // Validate name
            // not null names and match pattern
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new InvalidSpecialtyNameException("The specialty name cannot be empty.");
            }

            name = name.Trim();

            if (!NamePattern().IsMatch(name))
            {
                throw new InvalidSpecialtyNameException("The specialty name contains invalid characters or has an invalid length.");
            }

            // Validate treatment list is not empty
            if (!treatments.Any())
            {
                throw new EmptyTreatmentListException();
            }

            // if description is not null validate
            if (description is not null)
            {
                if (!DescriptionPattern().IsMatch(description))
                {
                    throw new InvalidSpecialtyDescriptionException();
                }
            }


            // Validate treatments uniqueness in the list
            HashSet<string> seenNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
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
            Description = description ?? string.Empty;
            _treatments.AddRange(treatments);
            Status = EntityStatus.Active;
        }


        public void AddTreatment(Treatment newTreatment)
        {
            // if treatment is null
            ArgumentNullException.ThrowIfNull(newTreatment);

            // specialty is not active

            if (Status != EntityStatus.Active)
            {
                throw new InvalidSpecialtyStateException();
            }

            // Name already exists
            if (_treatments.Any(t => t.Name.Equals(newTreatment.Name, StringComparison.OrdinalIgnoreCase)))
            {
                throw new DuplicateTreatmentNameException();
            }


            _treatments.Add(newTreatment);
        }


        public void RemoveTreatment(Guid treatmentId)
        {
            // When specialty is not active
            if (Status != EntityStatus.Active)
            {
                throw new InvalidSpecialtyStateException();
            }

            // When treatment is not found
            Treatment foundTreatment = _treatments
               .FirstOrDefault(t => t.TreatmentId == treatmentId)
               ?? throw new TreatmentNotFoundException(
                   $"Treatment with ID {treatmentId} was not found in this specialty.");

            // When treatment is already inactive
            if (foundTreatment.Status != EntityStatus.Active)
            {
                throw new TreatmentAlreadyInactiveException();
            }

            // When is the last active treatment
            if (_treatments.Count(t => t.Status == EntityStatus.Active) == 1)
            {
                throw new MinimumSpecialtyTreatmentsException("A specialty can't have less than one treatment.");
            }

            // remove
            foundTreatment.Deactivate();
        }


        public void UpdateDescription(string? description = null)
        {
            // Specialty is not active
            if (Status != EntityStatus.Active)
            {
                throw new InvalidSpecialtyStateException(); 
            }

            if (!string.IsNullOrEmpty(description))
            {
                // description does not match pattern
                if (!DescriptionPattern().IsMatch(description))
                {
                    throw new InvalidSpecialtyDescriptionException(); 
                }
            }

            // change or clear
            Description = !string.IsNullOrEmpty(description) ? description : string.Empty;
        }


        public void UpdateTreatmentDetails(string? treatmentName = null, 
            decimal? treatmentBaseCost = 0.0m, string? treatmentDescription = null)
        {
            throw new NotImplementedException();
        }

        public void Deactivate()
        {
            // Check current status
            if (Status == EntityStatus.Inactive)
            {
                throw new InvalidStatusTransitionException();
            }

            // Set children to inactive
            _treatments
                .ForEach(t => t.Deactivate());
            Status = EntityStatus.Inactive;
        }


        public void CorrectName(string correctedName)
        {
            // If specialty is inactive
            if (Status == EntityStatus.Inactive)
                throw new InvalidSpecialtyStateException();

            // If is null or invalid
            if (string.IsNullOrWhiteSpace(correctedName)) 
                throw new InvalidSpecialtyNameException();

            correctedName = correctedName.Trim();

            // is a correct format?
            if (!NamePattern().IsMatch(correctedName))
                throw new InvalidSpecialtyNameException();

            // is identical
            if (string.Equals(Name, correctedName, StringComparison.OrdinalIgnoreCase))
                return;

            // Apply correction 
            Name = correctedName;
        }


        public void Reactivate()
        {
            // Is already active
            if (Status == EntityStatus.Active)
            {
                throw new InvalidStatusTransitionException("The specialty is already active.");
            }

            // reactivate treatments
            foreach (var treatment in _treatments)
            {
                treatment.Reactivate();
            }

            Status = EntityStatus.Active;
        }
    }
}
