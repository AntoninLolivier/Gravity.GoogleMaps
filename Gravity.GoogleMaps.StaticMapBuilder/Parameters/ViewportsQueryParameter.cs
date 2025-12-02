using System.Globalization;

namespace Gravity.GoogleMaps.StaticMapBuilder.Parameters;

public class ViewportsQueryParameter : IQueryParameter
{
    public string Key { get; } = "visible";

    public bool IsUnique { get; } = false;
    
    public ParameterValue Value { get; }
    
    // Constructors
    
    public ViewportsQueryParameter(string[] locations)
    {
        Value = new ListParameterValue(locations, '|');
    }

    public ViewportsQueryParameter((double latitude, double longitude)[] coordinates)
    {
        List<string> coordinatesStrings = coordinates
            .Select(coord =>
                $"{coord.latitude.ToString(CultureInfo.InvariantCulture)},{coord.longitude.ToString(CultureInfo.InvariantCulture)}").ToList();
        Value = new ListParameterValue(coordinatesStrings, '|');
    }
}