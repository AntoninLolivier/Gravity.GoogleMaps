using System.Globalization;

namespace Gravity.GoogleMaps.TimeZoneBuilder.Parameters;

/// <summary>
/// The timestamp query parameter of the TimeZone API query
/// </summary>
/// <param name="timestamp">The timestamp from when to get the timezone.</param>
public class TimestampQueryParameter(DateTimeOffset timestamp) : IQueryParameter
{
    /// <inheritdoc />
    public string Key => "timestamp";

    /// <inheritdoc />
    public bool IsUnique => true;

    /// <inheritdoc />
    public ParameterValue Value { get; } = new SimpleParameterValue(timestamp.ToUnixTimeSeconds().ToString(CultureInfo.InvariantCulture));
}