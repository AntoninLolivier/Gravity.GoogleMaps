using System.Globalization;
using FluentValidation.Results;

namespace Gravity.GoogleMaps.TimeZoneBuilder.Parameters;

public class TimestampQueryParameter(DateTimeOffset value) : IQueryParameter
{
    /// <inheritdoc />
    public string Key { get; } = "timestamp";

    /// <inheritdoc />
    public bool IsUnique { get; } = true;

    /// <inheritdoc />
    public ParameterValue Value { get; } = new SimpleParameterValue(value.ToUnixTimeSeconds().ToString(CultureInfo.InvariantCulture));

    /// <inheritdoc />
    public ValidationResult Validate()
    {
        return new ValidationResult();
    }
}