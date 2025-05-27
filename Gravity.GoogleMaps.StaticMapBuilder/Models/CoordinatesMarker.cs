using System.Globalization;

namespace Gravity.GoogleMaps.StaticMapBuilder.Models;

/// <summary>
/// Represents a marker to be used in a static Google Maps image with defined geographical coordinates.
/// </summary>
public class CoordinatesMarker : Marker
{
    // Backing Fields
    
    private readonly double _latitude;
    private readonly double _longitude;
    

    // Properties
    
    /// <value>
    /// The latitude of the marker location, ranging from -90 to 90 degrees.
    /// </value>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the latitude is out of range. Should be between (-90 and 90)</exception>
    public double Latitude
    {
        get => _latitude;
        init
        {
            if (value is < -90 or > 90) throw new ArgumentOutOfRangeException(nameof(Latitude), ExceptionMessages.MalformedParametersExceptionMessages.LatitudeOutOfRangeMessage);
            
            _latitude = value;
        }
    }

    /// <value>
    /// The longitude of the marker location, ranging from -180 to 180 degrees.
    /// </value>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the longitude is out of range. Should be between (-180 and 180).</exception>
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

    /// <summary>
    /// Represents a marker on a Google Static Map defined by latitude and longitude coordinates.
    /// </summary>
    /// <remarks>
    /// This class supports all standard styling options and allows precise placement using geographic coordinates.
    /// Inherits all rendering rules and validation from the base <see cref="Marker"/> class.
    /// </remarks>
    /// <param name="longitude">
    /// The longitude of the marker location, ranging from -180 to 180 degrees.
    /// </param>
    /// <param name="latitude">
    /// The latitude of the marker location, ranging from -90 to 90 degrees.
    /// </param>
    /// <param name="size">
    /// Optional marker size (default is <see cref="MarkerSize.Default"/>).
    /// </param>
    /// <param name="color">
    /// Optional marker color. Can be a predefined <see cref="StaticMapColor"/> or a custom <see cref="HexColor"/> (without alpha).
    /// </param>
    /// <param name="label">
    /// Optional label (1 alphanumeric character). Cannot be used with <see cref="MarkerSize.Tiny"/> or <see cref="MarkerSize.Small"/>.
    /// </param>
    /// <param name="markerScale">
    /// Optional scale factor. Use <see cref="MarkerScale.Two"/> for high-DPI (retina) markers.
    /// </param>
    /// <param name="anchor">
    /// Optional anchor for custom icon positioning, either a <see cref="MarkerAnchor"/> value or pixel coordinates (x, y).
    /// Only applicable when <paramref name="iconUrl"/> is set.
    /// </param>
    /// <param name="iconUrl">
    /// Optional custom marker icon URL. Must be a fully qualified public URL.
    /// </param>
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

    /// <inheritdoc />
    public override string ToString()
    {
        string style = base.ToString();
        
        return string.IsNullOrEmpty(style) 
            ? $"{Latitude.ToString(CultureInfo.InvariantCulture)},{Longitude.ToString(CultureInfo.InvariantCulture)}" 
            : $"{style}|{Latitude.ToString(CultureInfo.InvariantCulture)},{Longitude.ToString(CultureInfo.InvariantCulture)}";
    }
}