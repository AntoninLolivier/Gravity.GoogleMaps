namespace Gravity.GoogleMaps.StaticMapBuilder.Strategies.Contracts;

internal interface IParameterFormattingStrategy
{
    IReadOnlyList<string> FormatValues(List<string> values);
}