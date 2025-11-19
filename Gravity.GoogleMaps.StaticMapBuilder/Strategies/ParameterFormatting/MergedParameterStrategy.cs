namespace Gravity.GoogleMaps.StaticMapBuilder.Strategies.ParameterFormatting;

internal class MergedParameterStrategy : IParameterFormattingStrategy
{
    IReadOnlyList<string> IParameterFormattingStrategy.FormatValues(List<string> values)
    {
        return [string.Join("|", values)];    
    }
}