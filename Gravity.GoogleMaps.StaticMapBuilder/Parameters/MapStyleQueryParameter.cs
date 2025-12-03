namespace Gravity.GoogleMaps.StaticMapBuilder.Parameters;

/// <summary>
/// A map style query parameter of the Static Map API query
/// </summary>
/// <param name="style">The style to apply to the map to customize the map appearance.</param>
public class MapStyleQueryParameter(MapStyle style) : IQueryParameter
{
    /// <inheritdoc />
    public string Key => "style";

    /// <inheritdoc />
    public bool IsUnique => false;

    /// <inheritdoc />
    public ParameterValue Value { get; } = new SimpleParameterValue(style.ToString());
}