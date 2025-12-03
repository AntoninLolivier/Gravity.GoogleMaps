
namespace Gravity.GoogleMaps.StaticMapBuilder.Parameters;

/// <summary>
/// The region query parameter of the Static Map API query.
/// </summary>
/// <param name="region">The region of the static map.</param>
public class RegionQueryParameter(string region) : IQueryParameter
{
    /// <inheritdoc />
    public string Key => "region";

    /// <inheritdoc />
    public bool IsUnique => true;

    /// <inheritdoc />
    public ParameterValue Value { get; } = new SimpleParameterValue(region);
}