namespace Gravity.GoogleMaps.Core.Parameters.Common;

/// <summary>
/// 
/// </summary>
/// <param name="rawValue"></param>
public class ApiKeyQueryParameter(string rawValue) : IQueryParameter
{
    /// <inheritdoc />
    public string Key => "key";

    /// <inheritdoc />
    public bool IsUnique => true;

    /// <inheritdoc />
    public ParameterValue Value { get; } = new SimpleParameterValue(rawValue);
}

