namespace Gravity.GoogleMaps.StaticMapBuilder.Strategies.ParameterFormatting;

internal class SeparatedParameterStrategy : IParameterFormattingStrategy // DELETE : On Next major
{
    IReadOnlyList<string> IParameterFormattingStrategy.FormatValues(List<string> values)
    {
        return values;
    }
}