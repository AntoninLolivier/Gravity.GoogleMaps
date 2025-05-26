namespace Gravity.GoogleMaps.StaticMapBuilder.Models;

public class CoordinatesMarker(
    double latitude,
    double longitude,
    MarkerSize size = MarkerSize.Default,
    OneOf<StaticMapColor, HexColor>? color = null,
    char? label = null,
    MarkerScale markerScale = MarkerScale.One,
    OneOf<MarkerAnchor, short>? anchor = null,
    string? iconUrl = null) : Marker(size, color, label, markerScale, anchor, iconUrl)
{
    // Backing Fields
    
    private readonly double _latitude = latitude;
    private readonly double _longitude = longitude;
    
    // Properties
    
    public double Latitude
    {
        get => _latitude;
        init
        {
            if (value is < -90 or > 90) throw new ArgumentOutOfRangeException(nameof(Latitude), ExceptionMessages.MalformedParametersExceptionMessages.LatitudeOutOfRangeMessage);
            
            _latitude = value;
        }
    }

    public double Longitude
    {
        get => _longitude;
        init
        {
            if (Longitude is < -180 or > 180) throw new ArgumentOutOfRangeException(nameof(Longitude), ExceptionMessages.MalformedParametersExceptionMessages.LongitudeOutOfRangeMessage);
            
            _longitude = value;
        }
    }
    
    // Methods

    public override string ToString()
    {
        string style = base.ToString();
        
        return $"${style}|{Latitude},{Longitude}";
    }
}