#pragma warning disable CS1591 
namespace Gravity.GoogleMaps.StaticMapBuilder.Options;

/// <summary>
/// Collection of available map formats.
/// </summary>
public static class MapFormats
{
    public static readonly MapFormat Png = new("png");
    public static readonly MapFormat Png32 = new("png32");
    public static readonly MapFormat Gif = new("gif");
    public static readonly MapFormat Jpg = new("jpg");
    public static readonly MapFormat JpgBaseline = new("jpg-baseline");
}