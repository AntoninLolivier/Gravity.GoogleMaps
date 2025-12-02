namespace Gravity.GoogleMaps.StaticMapBuilder.Parameters;

public class ScaleQueryParameter(MapScale value) : IQueryParameter
{
    public string Key => "scale";

    public bool IsUnique => true;

    public ParameterValue Value { get; } = new SimpleParameterValue(((int)value).ToString());
}