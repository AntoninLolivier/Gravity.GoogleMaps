namespace Gravity.GoogleMaps.StaticMapBuilder.Models;

public class MarkerGroup(
    MarkerSize size,
    OneOf<StaticMapColor, HexColor>? color = null,
    char? label = null,
    MarkerScale markerScale = MarkerScale.One,
    OneOf<MarkerAnchor, short>? anchor = null,
    string? iconUrl = null)
    : Marker(size, color, label, markerScale, anchor, iconUrl)
{
    // Fields
    
    private readonly List<string> _locations = [];
    
    // Properties
    
    internal int LocationCount;
    
    // Methods

    public void AddLocation(string location)
    {
        _locations.Add(location);
        LocationCount++;       
    }
    
    public void AddCoordiantes(double latitude, double longitude)
    {
        _locations.Add($"{latitude},{longitude}");
    }
    
    public override string ToString()
    {
        string style = base.ToString();
        
        return $"${style}|{string.Join("|", _locations)}";
    }
}