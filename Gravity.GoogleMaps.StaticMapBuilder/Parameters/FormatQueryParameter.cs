namespace Gravity.GoogleMaps.StaticMapBuilder.Parameters;

/// <summary>
/// The format parameter of the Static Map query parameter
/// </summary>
/// <param name="format">The format of the static map.</param>
public class FormatQueryParameter(MapFormat format) : IQueryParameter
{
    /// <inheritdoc />
    public string Key => "format";

    /// <inheritdoc />
    public bool IsUnique => true;

    /// <inheritdoc />
    public ParameterValue Value { get; } = new SimpleParameterValue(format.ToString().ToLower());
}