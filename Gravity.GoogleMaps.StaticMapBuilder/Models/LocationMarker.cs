namespace Gravity.GoogleMaps.StaticMapBuilder.Models;

public class LocationMarker(
    string location,
    MarkerSize size = MarkerSize.Default,
    OneOf<StaticMapColor, HexColor>? color = null,
    char? label = null,
    MarkerScale markerScale = MarkerScale.One,
    OneOf<MarkerAnchor, short>? anchor = null,
    string? iconUrl = null) : Marker(size, color, label, markerScale, anchor, iconUrl)
{
    
    public override string ToString()
    {
        string style = base.ToString();

        return $"${style}|{location}";
    }
}