using DentalSystem.Domain.Exceptions.ValueObjects;
using System.Text.RegularExpressions;

namespace DentalSystem.Domain.ValueObjects
{
    public sealed partial class Name : IEquatable<Name>
    {
        public string Value { get; }

        [GeneratedRegex(
            "^[a-zA-Z0-9áéíóúñÁÉÍÓÚÑ][a-zA-Z0-9áéíóúñÁÉÍÓÚÑ\\s-]{3,100}[a-zA-Z0-9áéíóúñÁÉÍÓÚÑ]$",
            RegexOptions.Compiled)]
        private static partial Regex NamePattern();

        public Name(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new InvalidNameException("Name cannot be empty.");

            value = value.Trim();

            if (!NamePattern().IsMatch(value))
                throw new InvalidNameException("Name contains invalid characters or has an invalid length.");

            Value = value;
        }

        // Equality by value (case-insensitive)
        public bool Equals(Name? other)
        {
            if (other is null) return false;
            return string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object? obj)
           => Equals(obj as Name);

        public override int GetHashCode()
            => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);

        public override string ToString() => Value;

     
        public static implicit operator string(Name name) => name.Value;

    }
}
