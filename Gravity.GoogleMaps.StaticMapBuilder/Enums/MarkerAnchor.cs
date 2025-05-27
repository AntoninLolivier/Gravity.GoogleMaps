#pragma warning disable CS1591 
namespace Gravity.GoogleMaps.StaticMapBuilder.Enums;

/// <summary>
/// Pre-defined markers anchor for custom icons.
/// </summary>
/// <remarks>
/// See <see href="https://developers.google.com/maps/documentation/maps-static/start#CustomIcons">official documentation</see>
/// for details.
/// </remarks>
public enum MarkerAnchor
{
    Top,
    Bottom,
    Left,
    Right,
    Center,
    TopLeft,
    TopRight,
    BottomLeft,
    BottomRight
}