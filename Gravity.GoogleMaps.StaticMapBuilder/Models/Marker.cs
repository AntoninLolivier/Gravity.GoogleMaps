namespace Gravity.GoogleMaps.StaticMapBuilder.Models;

public abstract class Marker
{
    // Backing field
    
    private readonly OneOf<StaticMapColor, HexColor>? _color;
    private readonly OneOf<MarkerAnchor, (int x, int y)>? _anchor;

    // Properties
    public char? Label { get; }
    
    public MarkerScale Scale { get; }
    
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