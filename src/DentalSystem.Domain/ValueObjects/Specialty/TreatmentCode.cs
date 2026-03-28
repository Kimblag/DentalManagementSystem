using DentalSystem.Domain.Exceptions;
using System.Text.RegularExpressions;

namespace DentalSystem.Domain.ValueObjects.Specialty
{
    public sealed partial record TreatmentCode
    {
        public readonly string Value;

        [GeneratedRegex("^\\d{2}\\.\\d{2}$", RegexOptions.Compiled)]
        private static partial Regex TreatmentCodePattern();

        public TreatmentCode(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainValidationException("The treatment code is mandatory.");

            value = value.Trim();

            if (!TreatmentCodePattern().IsMatch(value))
                throw new DomainValidationException("The treatment code is invalid.");

            Value = value;
        }
    }
}
