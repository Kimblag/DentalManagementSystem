using DentalSystem.Domain.Enums;
using DentalSystem.Domain.Exceptions;
using System.Text.RegularExpressions;

namespace DentalSystem.Domain.Entities
{
    public partial class Treatment
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; } = string.Empty;
        public decimal BaseCost { get; private set; } = 0;
        public EntityStatus Status { get; private set; }


        [GeneratedRegex("^[a-zA-Z0-9áéíóúñÁÉÍÓÚÑ][a-zA-Z0-9áéíóúñÁÉÍÓÚÑ\\s-]{1,98}[a-zA-Z0-9áéíóúñÁÉÍÓÚÑ]$")]
        private static partial Regex NamePattern();

        [GeneratedRegex("^[\\w\\sáéíóúñÁÉÍÓÚÑ\\.,;:\\-\\(\\)¿?¡!]{0,500}$")]
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
            else if (!NamePattern().IsMatch(name))
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

            Name = name;
            Description = description ?? string.Empty;
            BaseCost = baseCost;
            Status = EntityStatus.Active;
        }
    }
}
