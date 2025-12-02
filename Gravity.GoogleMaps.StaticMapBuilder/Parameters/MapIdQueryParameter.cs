namespace Gravity.GoogleMaps.StaticMapBuilder.Parameters;

public class MapIdQueryParameter(string mapId) : IQueryParameter
{
    public string Key => "map_id";

    public bool IsUnique => true;

    public ParameterValue Value { get; } = new SimpleParameterValue(mapId);
}