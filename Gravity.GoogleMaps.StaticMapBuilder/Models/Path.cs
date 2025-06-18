using System.Diagnostics;
using System.Globalization;

namespace Gravity.GoogleMaps.StaticMapBuilder.Models;

/// <summary>
/// Represents a path in a static map, which can be composed of either polylines or points.
/// </summary>
/// <remarks>
/// See <see href="https://developers.google.com/maps/documentation/maps-static/start#Paths">official documentation</see>
/// </remarks>
public record Path
{
    // Fields

    private int _pointsCount;
    private bool _hasPolyLine;
    
    // Properties
   
    /// <value>
    /// Specifies the thickness of the path in pixels. If no weight parameter is set, the path will appear in its default thickness (5 pixels).
    /// </value>
    public int Weight { get; }
    
    /// <value>
    /// Specifies the color of the path. Can be a predefined <see cref="StaticMapColor"/> or a custom <see cref="HexColor"/>.
    /// </value> 
    public OneOf<StaticMapColor, HexColor>? Color { get; }
    
    /// <value>
    /// Indicates both that the path marks off a polygonal area and specifies the fill color to use as an overlay within that area.
    /// The set of locations following need not be a "closed" loop;
    /// the Maps Static API server will automatically join the first and last points.
    /// Note, however, that any stroke on the exterior of the filled area will not be closed unless you specifically provide the same beginning and end location.
    /// </value>
    public OneOf<StaticMapColor, HexColor>? FillColor { get; }
    
    /// <value>
    /// Indicates that the requested path should be interpreted as a geodesic line that follows the curvature of the earth.
    /// When false, the path is rendered as a straight line in screen space. Defaults to false.
    /// </value>
    public bool Geodesic { get; }
    
    /// <value>
    /// The list of points that define the path.
    /// </value>
    public List<string>? Points { get; private set; }
    
    /// <value>
    /// An encoded polyline that defines the path. (Can't be combined with <see cref="Points"/>)
    /// </value>
    /// <remarks>
    /// See <see href="https://developers.google.com/maps/documentation/maps-static/start#EncodedPolylines">official documentation</see> for more information.
    /// </remarks>
    public string? Polyline { get; private set; }

    internal int LocationCount;

    // Constructor

    /// <summary>
    /// Represents a path on a static map with customizable properties such as weight, color, fill color, and geodesic.
    /// </summary>
    /// <remarks>
    /// The weight of the path is defined in pixels and must be between 0 and 500. If an invalid weight is provided, a default value of 5 is used.
    /// Geodesic determines if the path is drawn as the shortest path between points over the Earth's surface.
    /// </remarks>
    /// <param name="Weight">The stroke weight in pixels.</param>
    /// <param name="Color">The color of the path, defined as either a predefined static map color or a hexadecimal color value.</param>
    /// <param name="FillColor">The fill color for the path, specified as a static map color or a hexadecimal color value.</param>
    /// <param name="Geodesic">Indicates whether the path is drawn as a geodesic.</param>
    public Path(
        int Weight = 5,
        OneOf<StaticMapColor, HexColor>? Color = null,
        OneOf<StaticMapColor, HexColor>? FillColor = null,
        bool Geodesic = false)
    {
        if (Weight is < 0 or > 500)
        {
            Debug.WriteLine($"Warning: Warning The weight path style descriptor must be between 0 and 500. The resulting static map will " +
                            $"use weight 5 if the weight is outside of this range." +
                            $" See : {ProjectConstants.StaticMapDocumentationLinks.SectionLinks.PathStyles} for more information." +
                            $"Note that this range is not explicitly stated in the documentation, but was inferred from dev testing.");
        }
        
        this.Weight = Weight;
        this.Color = Color;
        this.FillColor = FillColor;
        this.Geodesic = Geodesic;
    }

    // Methods

    /// <summary>
    /// Adds a geographic point to the path using the specified latitude and longitude.
    /// </summary>
    /// <remarks>
    /// Latitude and longitude values must fall within their respective valid ranges. Adding points is incompatible with defining the path using a polyline.
    /// </remarks>
    /// <param name="latitude">The latitude of the point, which must be a value between -90 and 90 degrees inclusive.</param>
    /// <param name="longitude">The longitude of the point, which must be a value between -180 and 180 degrees inclusive.</param>
    /// <exception cref="ArgumentException">Thrown when the path is already defined by a polyline.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the latitude or longitude values are out of their valid ranges.</exception>
    public void AddPoint(double latitude, double longitude)
    {
        if (_hasPolyLine)
            throw new ArgumentException(ExceptionMessages.UrlParametersExceptionMessages
                .PathCannotBeDefinedByPointsAndPolylineExceptionMessage);
        if (longitude is < -180 or > 180) throw new ArgumentOutOfRangeException(nameof(longitude), ExceptionMessages.MalformedParametersExceptionMessages.LongitudeOutOfRangeMessage);
        if (latitude is < -90 or > 90) throw new ArgumentOutOfRangeException(nameof(latitude), ExceptionMessages.MalformedParametersExceptionMessages.LatitudeOutOfRangeMessage);
        
        string point = $"{latitude.ToString(CultureInfo.InvariantCulture)},{longitude.ToString(CultureInfo.InvariantCulture)}";

        Points ??= [];
        Points.Add(point);
        _pointsCount++;
    }

    /// <summary>
    /// Adds a point to the path, defined by its location name.
    /// </summary>
    /// <remarks>
    /// When a point is added, the location count and internal point counter is incremented. This method cannot be used after a polyline has been added to the path.
    /// </remarks>
    /// <param name="location">The name of the location to add to the path.</param>
    /// <exception cref="ArgumentException">Thrown if a polyline has already been added to the path.</exception>
    public void AddPoint(string location)
    {
        if (_hasPolyLine) throw new ArgumentException(ExceptionMessages.UrlParametersExceptionMessages.PathCannotBeDefinedByPointsAndPolylineExceptionMessage);
            
        Points ??= [];
        Points.Add(location);
        LocationCount++;
        _pointsCount++;
    }

    /// <summary>
    /// Adds a polyline to the path represented by an encoded string.
    /// </summary>
    /// <remarks>
    /// A path can either be defined by a polyline or a series of points, but not both. If points have already been added to the path, adding a polyline will throw an exception.
    /// </remarks>
    /// <param name="polyline">The encoded polyline string representing the path.</param>
    /// <exception cref="ArgumentException">Thrown when a path already contains points and an attempt is made to add a polyline.</exception>
    public void AddPolyline(string polyline)
    {
        if (_pointsCount > 0) throw new ArgumentException(ExceptionMessages.UrlParametersExceptionMessages.PathCannotBeDefinedByPointsAndPolylineExceptionMessage);
        
        Polyline = polyline;
        _hasPolyLine = true;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        List<string> pathStyles = [];
        
        if (Weight != 5)
        {
            pathStyles.Add($"weight:{Weight}");
        }

        Color?.Switch(
            baseColor => pathStyles.Add($"color:{baseColor.ToString().ToLower()}"),
            hexColor => pathStyles.Add($"color:{hexColor.ToString()}"));

        FillColor?.Switch(
            baseColor => pathStyles.Add($"fillcolor:{baseColor.ToString().ToLower()}"),
            hexColor => pathStyles.Add($"fillcolor:{hexColor.ToString()}"));

        if (Geodesic)
        {
            pathStyles.Add("geodesic:true");
        }
        
        string style = string.Join("|", pathStyles);

        if (_hasPolyLine)
        {
            return string.IsNullOrEmpty(style) ? $"enc:{Polyline}" : $"{style}|enc:{Polyline}";
        }
        
        if (Points is null || Points.Count < 2) throw new InvalidOperationException(ExceptionMessages.UrlParametersExceptionMessages.PathNeedAtLeastTwoPointsExceptionMessage);
        
        return string.IsNullOrEmpty(style) 
            ? string.Join("|", Points) 
            : $"{style}|{string.Join("|", Points)}";
    }
}