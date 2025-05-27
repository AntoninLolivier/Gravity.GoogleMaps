namespace Gravity.GoogleMaps.StaticMapBuilder.Models;

/// <summary>
/// Represents the abstract base for all Google Static Map markers.
/// </summary>
/// <remarks>
/// This class encapsulates common marker properties (color, label, icon, size, scale, and anchor).
/// Use <see cref="CoordinatesMarker"/>, <see cref="LocationMarker"/>, or <see cref="MarkerGroup"/> to instantiate specific marker types.
/// </remarks>
public abstract class Marker
{
    // Backing field
    
    private readonly OneOf<StaticMapColor, HexColor>? _color;
    private readonly OneOf<MarkerAnchor, (int x, int y)>? _anchor;

    // Properties
    
    /// <summary>
    /// Gets the single-character label to display on the marker.
    /// </summary>
    /// <remarks>
    /// Labels are limited to alphanumeric characters (A-Z, 0-9).
    /// Not allowed on <see cref="MarkerSize.Tiny"/> or <see cref="MarkerSize.Small"/> markers.
    /// </remarks>
    public char? Label { get; }
    
    /// <summary>
    /// Gets the scale factor of the marker.
    /// </summary>
    /// <value>Defaults to <see cref="MarkerScale.One"/>. Can be increased for high-DPI rendering.</value>
    public MarkerScale Scale { get; }
    
    /// <summary>
    /// Gets the size of the marker.
    /// </summary>
    /// <value>Can be <c>Default</c>, <c>Mid</c>, <c>Small</c>, or <c>Tiny</c>.</value>
    public MarkerSize Size { get; }
    
    internal string? IconUrl { get; }

    private OneOf<StaticMapColor, HexColor>? Color
    {
        get => _color;
        init
        {
            if (value is { IsT1: true, AsT1.IsAlphaSet: true })
            {
                throw new InvalidOperationException(ExceptionMessages.MalformedParametersExceptionMessages.AlphaCannotBeSetForMarkers);
            }
            _color = value;
        }
    }

    private OneOf<MarkerAnchor, (int x, int y)>? Anchor
    {
        get => _anchor;
        init
        {
            if (value is { IsT1: true})
            {
                (int x, int y) coordinates = value.Value.AsT1;
                if (coordinates.x is > 64 or < 0) throw new ArgumentOutOfRangeException(nameof(Anchor), ExceptionMessages.MalformedParametersExceptionMessages.MarkerAnchorXOutOfRangeMessage);
                if (coordinates.y is > 64 or < 0) throw new ArgumentOutOfRangeException(nameof(Anchor), ExceptionMessages.MalformedParametersExceptionMessages.MarkerAnchorYOutOfRangeMessage);
            }
            
            _anchor = value;
        }
    }

    // Constructor
    
    /// <summary>
    /// Initialize a new instance of the <see cref="Marker"/> class.
    /// </summary>
    /// <remarks>
    /// The <paramref name="color"/> must not have an alpha value.
    /// </remarks>
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
    /// <exception cref="InvalidOperationException">Thrown when the alpha is set in the color.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the anchor is set using x,y values and the x or y value is out of bound (must be between 0 and 64)</exception>
    protected Marker(
        MarkerSize size,
        OneOf<StaticMapColor, HexColor>? color,
        char? label,
        MarkerScale markerScale,
        OneOf<MarkerAnchor, (int, int)>? anchor,
        string? iconUrl)
    {
        Color = color;
        Size = size;
        Label = label;
        Scale = markerScale;
        Anchor = anchor;
        IconUrl = iconUrl;
    }
    
    // Methods
    
    /// <summary>
    /// Converts this marker to its query string representation, as expected by the Static Maps API.
    /// </summary>
    /// <returns>A properly formatted marker parameter string, such as <c>size:mid|color:blue|label:A|...</c>.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown if the label is used with unsupported marker sizes.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown if an anchor is defined without a custom icon URL.
    /// </exception> 
    public override string ToString()
    {
        List<string> markerStyles = [];

        if (Size is not MarkerSize.Default)
        {
            markerStyles.Add($"size:{Size.ToString().ToLower()}");
        }

        Color?.Switch(
            baseColor => markerStyles.Add($"color:{baseColor.ToString().ToLower()}"),
            hexColor => markerStyles.Add($"color:{hexColor.ToString()}"));

        if (Label is not null)
        {
            if (Size is MarkerSize.Tiny or MarkerSize.Small) throw new ArgumentException(ExceptionMessages.LabelNotSupportedExceptionMessage);
            markerStyles.Add($"label:{char.ToUpper(Label.Value)}");
        }
        
        if (Scale is not MarkerScale.One)
        {
            markerStyles.Add($"scale:{(int)Scale}");
        }

        Anchor?.Switch(
            anchor => markerStyles.Add($"anchor:{anchor.ToString().ToLower()}"),
            tupleAnchor => markerStyles.Add($"anchor:{tupleAnchor.ToString().Trim('(', ')').Replace(" ", "")}"));
        
        if (IconUrl is not null)
        {
            markerStyles.Add($"icon:{IconUrl}");
        }
        else if (Anchor is not null)
        {
            throw new InvalidOperationException(ExceptionMessages.UrlParametersExceptionMessages.AnchorCanBeSetOnlyForCustomIconsExceptionMessage);
        }
        
        return string.Join("|", markerStyles);
    }
}