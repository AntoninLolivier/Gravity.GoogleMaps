#pragma warning disable CS1591 
namespace Gravity.GoogleMaps.StaticMapBuilder.Enums;

/// <summary>
/// Visibility of an element of a <see cref="MapStyle"/>.
/// </summary>
/// <remarks>
/// See <see href="https://developers.google.com/maps/documentation/maps-static/styling#style-rules">official documentation</see>
/// for details.
/// </remarks>
public enum Visibility
{
    On,
    Off,
    Simplified
}