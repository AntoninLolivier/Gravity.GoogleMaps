#pragma warning disable CS1591 
namespace Gravity.GoogleMaps.StaticMapBuilder.Enums;

/// <summary>
/// Type for the static map.
/// </summary>
/// <remarks>
/// See <see href="https://developers.google.com/maps/documentation/maps-static/start#MapTypes">official documentation</see>
/// for details.
/// </remarks>
public enum StaticMapType
{
    Roadmap,
    Satellite,
    Terrain,
    Hybrid
}