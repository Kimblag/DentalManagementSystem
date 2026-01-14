using DentalSystem.Domain.Exceptions.ValueObjects;
using System.Text.RegularExpressions;

namespace DentalSystem.Domain.ValueObjects
{
    /// <summary>
    /// Represents a validated treatment name within the domain.
    /// </summary>
    /// <remarks>
    /// This value object ensures the name is not empty, falls between 5 and 102 characters 
    /// (including boundaries), and contains only valid alphanumeric characters.
    /// Equality is based on value and is case-insensitive.
    /// </remarks>
    public sealed partial class Name : IEquatable<Name>
    {
        public string Value { get; }

        [GeneratedRegex(
            "^[a-zA-Z0-9áéíóúñÁÉÍÓÚÑ][a-zA-Z0-9áéíóúñÁÉÍÓÚÑ\\s-]{3,100}[a-zA-Z0-9áéíóúñÁÉÍÓÚÑ]$",
            RegexOptions.Compiled)]
        private static partial Regex NamePattern();


        /// <summary>
        /// Initializes a new instance of the <see cref="Name"/> value object.
        /// </summary>
        /// <param name="value">The string value to validate and encapsulate.</param>
        /// <exception cref="InvalidNameException">
        /// Thrown when the value is null, empty, or does not match the required 
        /// alphanumeric pattern and length (3-100 characters plus boundaries).
        /// </exception>
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
        /// <summary>
        /// Compares the current instance with another <see cref="Name"/> object.
        /// </summary>
        /// <param name="other">The object to compare with this instance.</param>
        /// <returns>True if the name values match, ignoring case; otherwise, false.</returns>
        public bool Equals(Name? other)
        {
            if (other is null) return false;
            return string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current instance.
        /// </summary>
        /// <param name="obj">The object to compare.</param>
        /// <returns>True if the object is a <see cref="Name"/> and has the same value.</returns>
        public override bool Equals(object? obj)
           => Equals(obj as Name);

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code based on the case-insensitive value of the name.</returns>
        public override int GetHashCode()
            => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);

        /// <summary>
        /// Returns the name's value as a string.
        /// </summary>
        /// <returns>The string representation of the name.</returns>
        public override string ToString() => Value;

        /// <summary>
        /// Defines an implicit conversion of a <see cref="Name"/> to a <see cref="string"/>.
        /// </summary>
        /// <param name="name">The name object to convert.</param>
        public static implicit operator string(Name name) => name.Value;

    }
}
