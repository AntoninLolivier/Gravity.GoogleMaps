using System.Globalization;

namespace Gravity.GoogleMaps.StaticMapBuilder.Parameters;

public class CenterQueryParameter : IQueryParameter
{
    // Properties
    
    public string Key { get; } = "center";

    public bool IsUnique { get; } = true;
    
    public ParameterValue Value { get; }
    
    // Constructors

    public CenterQueryParameter(string locationCenter)
    {
        Value = new SimpleParameterValue(locationCenter);
    }
    
    public CenterQueryParameter(double latitude, double longitude)
    {
        Value = new SimpleParameterValue($"{latitude.ToString(CultureInfo.InvariantCulture)},{longitude.ToString(CultureInfo.InvariantCulture)}");
    }
}