namespace Gravity.GoogleMaps.StaticMapBuilder.Parameters;

public class FormatQueryParameter(MapFormat value) : IQueryParameter
{
    public string Key => "format";

    public bool IsUnique => true;

    public ParameterValue Value { get; } = new SimpleParameterValue(value.ToString().ToLower());
}