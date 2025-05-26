using System.Diagnostics;

namespace Gravity.GoogleMaps.StaticMapBuilder.Models;

public record Path
{
    // Fields

    private int _pointsCount;
    
    // Properties
    
    public int Weight { get; }
    
    public OneOf<StaticMapColor, HexColor>? Color { get; }
    
    public OneOf<StaticMapColor, HexColor>? FillColor { get; }
    
    public bool Geodesic { get; }
    
    public List<string>? Points { get; private set; }
    
    public string? Polyline { get; private set; }

    internal int LocationCount;
    
    internal bool HasPolyLine;
    
    // Constructor
    
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

    public void AddPoint(double latitude, double longitude)
    {
        if (HasPolyLine) throw new ArgumentException(ExceptionMessages.UrlParametersExceptionMessages.PathCannotBeDefinedByPointsAndPolylineExceptionMessage);
        if (longitude is < -180 or > 180) throw new ArgumentOutOfRangeException(nameof(longitude), ExceptionMessages.MalformedParametersExceptionMessages.LongitudeOutOfRangeMessage);
        if (latitude is < -90 or > 90) throw new ArgumentOutOfRangeException(nameof(latitude), ExceptionMessages.MalformedParametersExceptionMessages.LatitudeOutOfRangeMessage);
        
        string point = $"{latitude},{longitude}";

        Points ??= [];
        Points.Add(point);
        _pointsCount++;
    }

    public void AddPoint(string location)
    {
        if (HasPolyLine) throw new ArgumentException(ExceptionMessages.UrlParametersExceptionMessages.PathCannotBeDefinedByPointsAndPolylineExceptionMessage);
            
        Points ??= [];
        Points.Add(location);
        LocationCount++;
        _pointsCount++;
    }

    public void AddPolyline(string polyline)
    {
        if (_pointsCount > 0) throw new ArgumentException(ExceptionMessages.UrlParametersExceptionMessages.PathCannotBeDefinedByPointsAndPolylineExceptionMessage);
        
        Polyline = polyline;
        HasPolyLine = true;
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

        if (HasPolyLine)
        {
            return $"{style}|enc:{Polyline}";
        }
        
        if (Points is null || Points.Count < 2) throw new InvalidOperationException(ExceptionMessages.UrlParametersExceptionMessages.PathNeedAtLeastTwoPointsExceptionMessage);
        
        return $"{style}|{string.Join("|", Points)}";
    }
}