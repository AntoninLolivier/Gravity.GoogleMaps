namespace Gravity.GoogleMaps.StaticMapBuilder.Parameters;

/// <summary>
/// The size query parameter fo the Static Map API query.
/// </summary>
/// <param name="width">The width of the static map (in pixels)</param>
/// <param name="height">The height of the static map (in pixels)</param>
public class SizeQueryParameter(int width, int height) : IQueryParameter
{
    /// <inheritdoc />
    public string Key => "size";

    /// <inheritdoc />
    public bool IsUnique => true;

    /// <inheritdoc />
    public ParameterValue Value { get; } = new ListParameterValue([width.ToString(), height.ToString()], 'x');
}