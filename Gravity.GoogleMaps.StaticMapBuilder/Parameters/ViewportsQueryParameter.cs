using System.Globalization;

namespace Gravity.GoogleMaps.StaticMapBuilder.Parameters;

/// <summary>
/// The visible query parameter of the Static Map API Query.
/// </summary>
public class ViewportsQueryParameter : IQueryParameter
{
    // Properties

    /// <inheritdoc />
    public string Key => "visible";

    /// <inheritdoc />
    public bool IsUnique => false;

    /// <inheritdoc />
    public ParameterValue Value { get; }
    
    // Constructors
    
    /// <summary>
    /// Initialize a new instance of the <see cref="ViewportsQueryParameter"/> with human-readable locations.
    /// </summary>
    /// <param name="locations">Human-readable location that should be visible on the map.</param>
    public ViewportsQueryParameter(string[] locations)
    {
        Value = new ListParameterValue(locations, '|');
    }

    /// <summary>
    /// Initialize a new instance of the <see cref="ViewportsQueryParameter"/> with coordinates.
    /// </summary>
    /// <param name="coordinates">Coordinates of places that should be visible on the map.</param>
    public ViewportsQueryParameter((double latitude, double longitude)[] coordinates)
    {
        List<string> coordinatesStrings = coordinates
            .Select(coord =>
                $"{coord.latitude.ToString(CultureInfo.InvariantCulture)},{coord.longitude.ToString(CultureInfo.InvariantCulture)}").ToList();
        Value = new ListParameterValue(coordinatesStrings, '|');
    }
}