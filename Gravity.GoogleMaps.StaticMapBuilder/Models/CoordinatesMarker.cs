using System.Globalization;

namespace Gravity.GoogleMaps.StaticMapBuilder.Models;

public class CoordinatesMarker : Marker
{
    // Backing Fields
    
    private readonly double _latitude;
    private readonly double _longitude;
    

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
            if (value is < -180 or > 180) throw new ArgumentOutOfRangeException(nameof(Longitude), ExceptionMessages.MalformedParametersExceptionMessages.LongitudeOutOfRangeMessage);
            
            _longitude = value;
        }
    }
    
    // Constructor

    public CoordinatesMarker(
        double latitude,
        double longitude,
        MarkerSize size = MarkerSize.Default,
        OneOf<StaticMapColor, HexColor>? color = null,
        char? label = null,
        MarkerScale markerScale = MarkerScale.One,
        OneOf<MarkerAnchor, (int, int)>? anchor = null,
        string? iconUrl = null) : base(size, color, label, markerScale, anchor, iconUrl)
    {
        Latitude = latitude;
        Longitude = longitude;
    }
    
    // Methods

    public override string ToString()
    {
        string style = base.ToString();
        
        return string.IsNullOrEmpty(style) 
            ? $"{Latitude.ToString(CultureInfo.InvariantCulture)},{Longitude.ToString(CultureInfo.InvariantCulture)}" 
            : $"{style}|{Latitude.ToString(CultureInfo.InvariantCulture)},{Longitude.ToString(CultureInfo.InvariantCulture)}";
    }
}