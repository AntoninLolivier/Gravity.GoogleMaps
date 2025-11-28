namespace Gravity.GoogleMaps.Core.Parameters;

public record SimpleParameterValue(string Value) : ParameterValue
{
    public override string Value { get; } = Value;
}