

using DentalSystem.Domain.Enums;
using DentalSystem.Domain.Exceptions;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DentalSystem.Domain.Entities
{
    // partial because we need to tell .NET that we need to use a generated regex code
    public partial class Specialty
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; } = string.Empty;
        public EntityStatus Status { get; private set; }
        private readonly List<Treatment> _treatments = new();

        // This '=>' acts like a live mirror.It does not store data itself;
        // it simply reflects whatever is currently inside the private '_treatments' list.
        // It provides a 'glass window' that allows external users to see the treatments 
        // without being able to touch or modify the actual collection.
        // while allowing them to iterate and read the data.
        public IReadOnlyCollection<Treatment> Treatments => _treatments.AsReadOnly();

        [GeneratedRegex("^[a-zA-Z0-9áéíóúñÁÉÍÓÚÑ][a-zA-Z0-9áéíóúñÁÉÍÓÚÑ\\s-]{1,98}[a-zA-Z0-9áéíóúñÁÉÍÓÚÑ]$")]
        private static partial Regex NamePattern();

        [GeneratedRegex("^[\\w\\sáéíóúñÁÉÍÓÚÑ\\.,;:\\-\\(\\)¿?¡!]{0,500}$")]
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
                throw new InvalidSpecialtyNameException();
            } else if (!NamePattern().IsMatch(name))
            {
                throw new InvalidSpecialtyNameException();
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
                    throw new DuplicateTreatmentNameException();
                }
            }

            // Create instance
            Name = name;
            Description = description ?? string.Empty;
            _treatments.AddRange(treatments);
            Status = EntityStatus.Active;
        }
    }
}
