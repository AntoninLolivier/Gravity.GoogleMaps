using Path = Gravity.GoogleMaps.StaticMapBuilder.Models.Path;

namespace Gravity.GoogleMaps.StaticMapBuilder.Parameters;

public class PathQueryParameter(Path path) : IQueryParameter
{
    public string Key { get; } = "path";

    public bool IsUnique { get; } = false;

    public ParameterValue Value { get; } = new SimpleParameterValue(path.ToString());
}