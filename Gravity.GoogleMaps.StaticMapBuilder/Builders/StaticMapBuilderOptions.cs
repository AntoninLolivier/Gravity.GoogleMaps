namespace Gravity.GoogleMaps.StaticMapBuilder.Builders;

public record StaticMapBuilderOptions
{
    public bool DisableApiKeyCheck { get; init; } = false;
    
    public bool ReturnParametersOnly { get; init; } = false;
    
    public bool DisableUrlEncoding { get; init; } = false;
    
    public bool UseHttp { get; init; } = false;
}