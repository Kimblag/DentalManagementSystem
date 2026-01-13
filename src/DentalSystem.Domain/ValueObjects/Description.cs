using DentalSystem.Domain.Exceptions.ValueObjects;
using System.Text.RegularExpressions;

namespace DentalSystem.Domain.ValueObjects
{
    public sealed partial class Description
    {
        public string? Value { get; }

        [GeneratedRegex("^[\\w\\sáéíóúñÁÉÍÓÚÑ\\.,;:\\-\\(\\)¿?¡!]{3,500}$")]
        private static partial Regex Pattern();

        public Description(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                Value = null;
                return;
            }

            if (!Pattern().IsMatch(value))
                throw new InvalidDescriptionException("Description contains invalid characters or has an invalid length.");

            Value = value.Trim();
        }

        public override string ToString() => Value ?? string.Empty;
    }
}
