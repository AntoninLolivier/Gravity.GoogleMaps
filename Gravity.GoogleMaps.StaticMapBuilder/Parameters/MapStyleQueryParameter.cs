namespace Gravity.GoogleMaps.StaticMapBuilder.Parameters;

public class MapStyleQueryParameter(MapStyle style) : IQueryParameter
{
    public string Key { get; } = "style";
    
    public bool IsUnique { get; } = false;

    public ParameterValue Value { get; } = new SimpleParameterValue(style.ToString());
}