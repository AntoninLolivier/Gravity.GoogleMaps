using Path = Gravity.GoogleMaps.StaticMapBuilder.Models.Path;

namespace Gravity.GoogleMaps.StaticMapBuilder.Parameters;

/// <summary>
/// A path query parameter of the Static Map API query.
/// </summary>
/// <param name="path">The path to add to the static map.</param>
public class PathQueryParameter(Path path) : IQueryParameter
{
    /// <inheritdoc />
    public string Key => "path";

    /// <inheritdoc />
    public bool IsUnique => false;

    /// <inheritdoc />
    public ParameterValue Value { get; } = new SimpleParameterValue(path.ToString());
}