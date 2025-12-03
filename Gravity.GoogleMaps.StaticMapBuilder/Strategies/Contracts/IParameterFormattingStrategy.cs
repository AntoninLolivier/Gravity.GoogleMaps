namespace Gravity.GoogleMaps.StaticMapBuilder.Strategies.Contracts;

internal interface IParameterFormattingStrategy // DELETE : On next Major
{
    IReadOnlyList<string> FormatValues(List<string> values);
}