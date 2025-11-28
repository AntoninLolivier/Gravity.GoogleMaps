using System.Globalization;

namespace Gravity.GoogleMaps.TimeZoneBuilder.Parameters;

public class LocationQueryParameter(double latitude, double longitude) : IQueryParameter
{
    public string Key => "location";

    public bool IsUnique => true;

    public ParameterValue Value { get; } = new SimpleParameterValue($"{latitude.ToString(CultureInfo.InvariantCulture)},{longitude.ToString(CultureInfo.InvariantCulture)}");
}