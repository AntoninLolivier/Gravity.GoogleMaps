namespace Gravity.GoogleMaps.StaticMapBuilder.Strategies.ParameterFormatting;

internal class SimpleParameterStrategy : IParameterFormattingStrategy
{
    IReadOnlyList<string> IParameterFormattingStrategy.FormatValues(List<string> values)
    {
        return [values[0]];
    }
}