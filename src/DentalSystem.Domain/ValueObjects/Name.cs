using DentalSystem.Domain.Exceptions.ValueObjects;
using System.Text.RegularExpressions;

namespace DentalSystem.Domain.ValueObjects
{
    public sealed partial class Name : IEquatable<Name>
    {
        public readonly string Value;

        [GeneratedRegex(
            "^[a-zA-Z0-9áéíóúñÁÉÍÓÚÑ][a-zA-Z0-9áéíóúñÁÉÍÓÚÑ\\s-]{3,100}[a-zA-Z0-9áéíóúñÁÉÍÓÚÑ]$",
            RegexOptions.Compiled)]
        private static partial Regex NamePattern();

        public Name(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new InvalidNameException();

            value = value.Trim();

            if (!NamePattern().IsMatch(value))
                throw new InvalidNameException();

            Value = value;
        }


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

    }
}
