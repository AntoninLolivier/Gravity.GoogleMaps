namespace Gravity.GoogleMaps.StaticMapBuilder.Parameters;

public class SizeQueryParameter(int width, int height) : IQueryParameter
{
    public string Key => "size";

    public bool IsUnique => true;

    public ParameterValue Value { get; } = new ListParameterValue([width.ToString(), height.ToString()], 'x');
}