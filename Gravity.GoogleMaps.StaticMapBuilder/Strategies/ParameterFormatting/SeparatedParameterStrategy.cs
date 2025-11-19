namespace Gravity.GoogleMaps.StaticMapBuilder.Strategies.ParameterFormatting;

internal class SeparatedParameterStrategy : IParameterFormattingStrategy
{
    IReadOnlyList<string> IParameterFormattingStrategy.FormatValues(List<string> values)
    {
        return values;
    }
}