namespace Gravity.GoogleMaps.StaticMapBuilder.Strategies.ParameterFormatting;

internal class SimpleParameterStrategy : IParameterFormattingStrategy // DELETE : On enxt Major
{
    IReadOnlyList<string> IParameterFormattingStrategy.FormatValues(List<string> values)
    {
        return [values[0]];
    }
}