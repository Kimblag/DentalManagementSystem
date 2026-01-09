using DentalSystem.Domain.Enums;
using DentalSystem.Domain.Exceptions.Specialties;
using System.Text.RegularExpressions;

namespace DentalSystem.Domain.Entities
{
    public partial class Treatment
    {
        // Domain identity
        public Guid TreatmentId { get; private set; }

        // EF persistence Identity
        public int Id { get; private set; }
        public string Name { get; private set; } = null!;
        public string Description { get; private set; } = string.Empty;
        public decimal BaseCost { get; private set; } = 0;
        public EntityStatus Status { get; private set; }


        [GeneratedRegex("^[a-zA-Z0-9áéíóúñÁÉÍÓÚÑ][a-zA-Z0-9áéíóúñÁÉÍÓÚÑ\\s-]{3,100}[a-zA-Z0-9áéíóúñÁÉÍÓÚÑ]$")]
        private static partial Regex NamePattern();

        [GeneratedRegex("^[\\w\\sáéíóúñÁÉÍÓÚÑ\\.,;:\\-\\(\\)¿?¡!]{3,500}$")]
        private static partial Regex DescriptionPattern();

        private Treatment()
        {
            
        }

        public Treatment(string name, decimal baseCost, string? description)
        {
            // Check name
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new InvalidTreatmentNameException();
            }
            name = name.Trim();
            if (!NamePattern().IsMatch(name))
            {
                throw new InvalidTreatmentNameException();
            }


            // check base cost
            if (baseCost < 0)
            {
                throw new InvalidTreatmentCostException();
            }


            // if description is not null validate
            if (description is not null)
            {
                if (!DescriptionPattern().IsMatch(description))
                {
                    throw new InvalidTreatmentDescriptionException();
                }
            }

            TreatmentId = Guid.NewGuid();
            Name = name;
            Description = description ?? string.Empty;
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

            if (string.IsNullOrWhiteSpace(correctedName))
            {
                throw new InvalidTreatmentNameException();
            }
            correctedName = correctedName.Trim();

            // invalid name
            if (!NamePattern().IsMatch(correctedName))
            {
                throw new InvalidTreatmentNameException();
            }

            // is identical
            if (string.Equals(Name, correctedName, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            // correct name
            Name = correctedName;
        }

        internal void UpdateDetails(decimal? newBaseCost = null, string? newDescription = null)
        {
            // if there is a new base cost
            if (newBaseCost.HasValue && newBaseCost < 0)
            {
               throw new InvalidTreatmentCostException();
            }

            if (newDescription is not null && !DescriptionPattern().IsMatch(newDescription))
            {
                throw new InvalidTreatmentDescriptionException("Description contains invalid characters or length.");
            }

            BaseCost = newBaseCost ?? BaseCost;
            Description = newDescription ?? Description;
        }


    }
}
