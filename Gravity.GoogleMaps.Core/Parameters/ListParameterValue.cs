namespace Gravity.GoogleMaps.Core.Parameters;

public record ListParameterValue(IEnumerable<string> Values, char Separator = '|') : ParameterValue
{
    public override string Value => string.Join(Separator, Values);
}