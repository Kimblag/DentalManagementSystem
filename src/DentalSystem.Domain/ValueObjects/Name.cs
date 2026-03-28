using DentalSystem.Domain.Exceptions;
using System.Text.RegularExpressions;

namespace DentalSystem.Domain.ValueObjects
{
    public sealed partial record Name
    {
        public string Value { get; init; }

        [GeneratedRegex(
            "^[a-zA-Z0-9áéíóúñÁÉÍÓÚÑ][a-zA-Z0-9áéíóúñÁÉÍÓÚÑ\\s\\.-]{1,100}[a-zA-Z0-9áéíóúñÁÉÍÓÚÑ\\.]$",
        RegexOptions.Compiled)]
        private static partial Regex NamePattern();

        public Name(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainValidationException("Name is mandatory.");

            value = value.Trim();

            if (!NamePattern().IsMatch(value))
                throw new DomainValidationException("The name's format is invalid.");

            Value = value;
        }
    }
}
