namespace Gravity.GoogleMaps.StaticMapBuilder.Parameters;

/// <summary>
/// The map type query parameter of the Static Map API query
/// </summary>
/// <param name="mapType">The static map type.</param>
public class MapTypeQueryParameter(StaticMapType mapType) : IQueryParameter
{
    /// <inheritdoc />
    public string Key => "maptype";

    /// <inheritdoc />
    public bool IsUnique => true;

    /// <inheritdoc />
    public ParameterValue Value { get; } = new SimpleParameterValue(mapType.ToString().ToLower());
}