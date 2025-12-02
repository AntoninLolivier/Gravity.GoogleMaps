namespace Gravity.GoogleMaps.StaticMapBuilder.Parameters;

public class ZoomQueryParameter(int value) : IQueryParameter
{
    public string Key => "zoom";

    public bool IsUnique => true;

    public ParameterValue Value { get; } = new SimpleParameterValue(value.ToString());
}