namespace Gravity.GoogleMaps.StaticMapBuilder.Models;

public abstract class Marker(
    MarkerSize size,
    OneOf<StaticMapColor, HexColor>? color,
    char? label,
    MarkerScale markerScale,
    OneOf<MarkerAnchor, short>? anchor,
    string? iconUrl)
{
    // Backing field
    
    private readonly OneOf<StaticMapColor, HexColor>? _color = color;
    
    // Properties
    
    public MarkerSize Size { get; } = size;

    public OneOf<StaticMapColor, HexColor>? Color
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

    public char? Label { get; } = label;
    
    public MarkerScale MarkerScale { get; } = markerScale;
    
    public OneOf<MarkerAnchor, short>? Anchor { get; } = anchor;
    
    public string? IconUrl { get; } = iconUrl;

    // Methods
    
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
        
        if (MarkerScale is not MarkerScale.One)
        {
            markerStyles.Add($"scale:{MarkerScale}");
        }

        Anchor?.Switch(
            anchor => markerStyles.Add($"anchor:{anchor}"),
            bitsAnchor => markerStyles.Add($"anchor:{bitsAnchor}"));

        if (IconUrl is not null)
        {
            markerStyles.Add($"icon:{IconUrl}");
        }
        
        return string.Join("|", markerStyles);
    }
}