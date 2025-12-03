namespace Gravity.GoogleMaps.StaticMapBuilder.Parameters;

/// <summary>
/// The map id parameter of the Static Map API query
/// </summary>
/// <param name="mapId">The identifier of the map to customize static map appearance.</param>
public class MapIdQueryParameter(string mapId) : IQueryParameter
{
    /// <inheritdoc />
    public string Key => "map_id";

    /// <inheritdoc />
    public bool IsUnique => true;

    /// <inheritdoc />
    public ParameterValue Value { get; } = new SimpleParameterValue(mapId);
}