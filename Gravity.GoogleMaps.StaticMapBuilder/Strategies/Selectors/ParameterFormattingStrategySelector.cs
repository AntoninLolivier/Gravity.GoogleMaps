using Gravity.GoogleMaps.StaticMapBuilder.Strategies.ParameterFormatting;

namespace Gravity.GoogleMaps.StaticMapBuilder.Strategies.Selectors;

internal static class ParameterFormattingStrategySelector
{
    internal static IParameterFormattingStrategy Choose(StaticMapRequestParameters parameterType)
    {
        return parameterType switch
        {
            StaticMapRequestParameters.Center 
                or StaticMapRequestParameters.Zoom 
                or StaticMapRequestParameters.Size
                or StaticMapRequestParameters.Scale 
                or StaticMapRequestParameters.Format
                or StaticMapRequestParameters.MapType 
                or StaticMapRequestParameters.Language
                or StaticMapRequestParameters.Region 
                or StaticMapRequestParameters.MapId
                or StaticMapRequestParameters.Key => new SimpleParameterStrategy(),
            StaticMapRequestParameters.Marker 
                or StaticMapRequestParameters.Path 
                or StaticMapRequestParameters.Style => new SeparatedParameterStrategy(),
            StaticMapRequestParameters.Visible => new MergedParameterStrategy(),
            _ => throw new ArgumentOutOfRangeException(nameof(parameterType), parameterType, null)
        };
    }
}