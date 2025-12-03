using System.Globalization;

namespace Gravity.GoogleMaps.TimeZoneBuilder.Parameters;

/// <summary>
/// The location query parameter of the TimeZone API query
/// </summary>
/// <param name="latitude">The latitude of the point to get timezone</param>
/// <param name="longitude">The longitude of the point to get timezone</param>
public class LocationQueryParameter(double latitude, double longitude) : IQueryParameter
{
    /// <inheritdoc />
    public string Key => "location";

    /// <inheritdoc />
    public bool IsUnique => true;

    /// <inheritdoc />
    public ParameterValue Value { get; } = new SimpleParameterValue($"{latitude.ToString(CultureInfo.InvariantCulture)},{longitude.ToString(CultureInfo.InvariantCulture)}");
}