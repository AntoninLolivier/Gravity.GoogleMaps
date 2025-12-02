namespace Gravity.GoogleMaps.StaticMapBuilder.Parameters;

public class MarkerQueryParameter(Marker marker) : IQueryParameter
{
    public string Key { get; } = "markers";

    public bool IsUnique { get; } = false;

    public ParameterValue Value { get; } = new SimpleParameterValue(marker.ToString());
}