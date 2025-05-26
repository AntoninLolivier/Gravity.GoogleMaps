namespace Gravity.GoogleMaps.StaticMapBuilder.Models.MapStyles;

public record MapStyle(
    StyleRule StyleRule,
    Feature? Feature,
    Element? Element)
{
    /// <inheritdoc />
    public override string ToString()
    {
        List<string> styleValues = [];
        
        if (Feature is not null)
        {
            styleValues.Add($"feature:{Feature.Value}");
        }

        if (Element is not null)
        {
            styleValues.Add($"element:{Element.Value}");
        }
        
        styleValues.Add(StyleRule.ToString());

        return string.Join('|', styleValues);
    }
}