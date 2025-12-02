namespace Gravity.GoogleMaps.StaticMapBuilder.Parameters;

public class MapTypeQueryParameter(StaticMapType value) : IQueryParameter
{
    public string Key => "maptype";

    public bool IsUnique => true;

    public ParameterValue Value { get; } = new SimpleParameterValue(value.ToString().ToLower());
}