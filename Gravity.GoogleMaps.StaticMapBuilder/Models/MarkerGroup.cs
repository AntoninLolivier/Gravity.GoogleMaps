using System.Globalization;

namespace Gravity.GoogleMaps.StaticMapBuilder.Models;

/// <summary>
/// A class used to define a single style for multiple markers.
/// </summary>
/// <param name="size">The size of the markers.</param>
/// <param name="color">The color of the markers</param>
/// <param name="label">The label in the markers.</param>
/// <param name="markerScale">The scale of the markers.</param>
/// <param name="anchor">The anchor of the markers.</param>
/// <param name="iconUrl">The icon url of the markers.</param>
public class MarkerGroup(
    MarkerSize size,
    OneOf<StaticMapColor, HexColor>? color = null,
    char? label = null,
    MarkerScale markerScale = MarkerScale.One,
    OneOf<MarkerAnchor, (int, int)>? anchor = null,
    string? iconUrl = null)
    : Marker(size, color, label, markerScale, anchor, iconUrl)
{
    // Fields
    
    private readonly List<string> _locations = [];
    
    // Properties
    
    internal int LocationCount;
    
    // Methods

    /// <summary>
    /// Add a location to the group.
    /// </summary>
    /// <param name="location">A marker as a location string.</param>
    public void AddLocation(string location)
    {
        _locations.Add(location);
        LocationCount++;       
    }
    
    /// <summary>
    /// Add coordinates to the group.
    /// </summary>
    /// <param name="latitude">The latitude of the new maker in the group.</param>
    /// <param name="longitude">The longitude of the new marker in the group.</param>
    public void AddCoordinates(double latitude, double longitude)
    {
        _locations.Add($"{latitude.ToString(CultureInfo.InvariantCulture)},{longitude.ToString(CultureInfo.InvariantCulture)}");
    }
    
    /// <inheritdoc />
    public override string ToString()
    {
        string style = base.ToString();
        
        return $"{style}|{string.Join("|", _locations)}";
    }
}