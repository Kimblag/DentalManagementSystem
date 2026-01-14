using DentalSystem.Domain.Exceptions.ValueObjects;
using System.Text.RegularExpressions;

namespace DentalSystem.Domain.ValueObjects
{
    /// <summary>
    /// Represents an optional detailed explanation or clinical notes for a treatment.
    /// </summary>
    /// <remarks>
    /// If provided, the description must be between 3 and 500 characters and can include 
    /// alphanumeric characters and basic punctuation. 
    /// An empty or whitespace string is treated as a null (empty) description.
    /// </remarks>
    public sealed partial class Description
    {
        public string? Value { get; }

        [GeneratedRegex("^[\\w\\sáéíóúñÁÉÍÓÚÑ\\.,;:\\-\\(\\)¿?¡!]{3,500}$")]
        private static partial Regex Pattern();


        /// <summary>
        /// Initializes a new instance of the <see cref="Description"/> value object.
        /// </summary>
        /// <param name="value">The description text. If null or whitespace, it results in an empty description.</param>
        /// <exception cref="InvalidDescriptionException">
        /// Thrown when the provided text contains invalid characters or does not meet 
        /// the length requirements (3-500 characters).
        /// </exception>
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


        /// <summary>
        /// Returns the description value or an empty string if it is null.
        /// </summary>
        /// <returns>A string representation of the description.</returns>
        public override string ToString() => Value ?? string.Empty;
    }
}
