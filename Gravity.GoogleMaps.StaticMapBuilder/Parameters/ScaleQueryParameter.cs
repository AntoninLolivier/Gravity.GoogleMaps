namespace Gravity.GoogleMaps.StaticMapBuilder.Parameters;

/// <summary>
/// The scale parameter of the Static Map API query
/// </summary>
/// <param name="scale">The scale of the static map.</param>
public class ScaleQueryParameter(MapScale scale) : IQueryParameter
{
    /// <inheritdoc />
    public string Key => "scale";

    /// <inheritdoc />
    public bool IsUnique => true;

    /// <inheritdoc />
    public ParameterValue Value { get; } = new SimpleParameterValue(((int)scale).ToString());
}