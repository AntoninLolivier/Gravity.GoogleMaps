namespace Gravity.GoogleMaps.Core.Parameters;

/// <summary>
/// A type of parameter that contain a simple value.
/// </summary>
/// <param name="Value">The value of the query parameter.</param>
public record SimpleParameterValue(string Value) : ParameterValue
{
    /// <inheritdoc />
    public override string Value { get; } = Value;
}