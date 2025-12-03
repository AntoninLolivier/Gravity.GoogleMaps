namespace Gravity.GoogleMaps.StaticMapBuilder.Parameters;

/// <summary>
/// A marker query parameter of the Static Map API query
/// </summary>
/// <param name="marker">The marker to add to the static map.</param>
public class MarkerQueryParameter(Marker marker) : IQueryParameter
{
    /// <inheritdoc />
    public string Key => "markers";

    /// <inheritdoc />
    public bool IsUnique => false;

    /// <inheritdoc />
    public ParameterValue Value { get; } = new SimpleParameterValue(marker.ToString());
}