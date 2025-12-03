namespace Gravity.GoogleMaps.StaticMapBuilder.Parameters;

/// <summary>
/// The zoom query parameter of the Static Map API query
/// </summary>
/// <param name="zoom">The zoom.</param>
public class ZoomQueryParameter(int zoom) : IQueryParameter
{
    /// <inheritdoc />
    public string Key => "zoom";

    /// <inheritdoc />
    public bool IsUnique => true;

    /// <inheritdoc />
    public ParameterValue Value { get; } = new SimpleParameterValue(zoom.ToString());
}