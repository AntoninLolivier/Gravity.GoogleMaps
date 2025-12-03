namespace Gravity.GoogleMaps.Core.Parameters;

/// <summary>
/// Base class to represent a query parameter value.
/// </summary>
public abstract record ParameterValue
{
    /// <value>
    /// The query parameters value as string.
    /// </value>
    public abstract string Value { get; }
}