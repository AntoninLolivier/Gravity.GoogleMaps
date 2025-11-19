namespace Gravity.GoogleMaps.StaticMapBuilder.Strategies.Decorators;

internal class UrlEncodingDecorator(IParameterFormattingStrategy innerStrategy) : IParameterFormattingStrategy
{
    IReadOnlyList<string> IParameterFormattingStrategy.FormatValues(List<string> values)
    {
        IReadOnlyList<string> raw = innerStrategy.FormatValues(values);
        
        return raw.Select(Uri.EscapeDataString).ToList();
    }
}