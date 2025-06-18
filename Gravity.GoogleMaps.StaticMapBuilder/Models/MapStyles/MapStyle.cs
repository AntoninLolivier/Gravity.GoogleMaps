namespace Gravity.GoogleMaps.StaticMapBuilder.Models.MapStyles;

/// <summary>
/// Represents a style specification for Google Maps Static API.
/// </summary>
/// <remarks>
/// A <see cref="MapStyle"/> is defined by a combination of <see cref="StyleRule"/>,
/// <see cref="Feature"/>, and <see cref="Element"/>, and is used to specify
/// how map features and elements are displayed. If only the style rule is specified, it is applied to the whole map.
/// </remarks>
/// <param name="StyleRule">The object containing the styling information</param>
/// <param name="Feature">The feature that where the <see cref="StyleRule"/> is applied.</param>
/// <param name="Element">The element of the feature where the <see cref="StyleRule"/> is applied.</param>
public record MapStyle(
    StyleRule StyleRule,
    Feature? Feature = null,
    Element? Element = null)
{
    /// <inheritdoc />
    public override string ToString()
    {
        List<string> styleValues = [];
        
        if (Feature is not null)
        {
            styleValues.Add($"feature:{Feature}");
        }

        if (Element is not null)
        {
            styleValues.Add($"element:{Element}");
        }
        
        styleValues.Add(StyleRule.ToString());

        return string.Join('|', styleValues);
    }
}