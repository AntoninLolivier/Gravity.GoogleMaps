
namespace Gravity.GoogleMaps.StaticMapBuilder.Parameters;

public class RegionQueryParameter(string value) : IQueryParameter
{
    public string Key => "region";

    public bool IsUnique => true;

    public ParameterValue Value { get; } = new SimpleParameterValue(value);
}