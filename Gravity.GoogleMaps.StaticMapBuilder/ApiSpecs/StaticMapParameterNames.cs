namespace Gravity.GoogleMaps.StaticMapBuilder.ApiSpecs;

[Obsolete] 
internal static class StaticMapParameterNames // DELETE : On next major
{
    internal static readonly IReadOnlyDictionary<StaticMapRequestParameters, string> Map =
        new Dictionary<StaticMapRequestParameters, string>
        {
            { StaticMapRequestParameters.Center, "center" },
            { StaticMapRequestParameters.Zoom, "zoom" },
            { StaticMapRequestParameters.Size, "size" },
            { StaticMapRequestParameters.Scale, "scale" },
            { StaticMapRequestParameters.Format, "format" },
            { StaticMapRequestParameters.MapType, "maptype" },
            { StaticMapRequestParameters.Language, "language" },
            { StaticMapRequestParameters.Region, "region" },
            { StaticMapRequestParameters.MapId, "map_id" },
            { StaticMapRequestParameters.Marker, "markers" },
            { StaticMapRequestParameters.Path, "path" },
            { StaticMapRequestParameters.Visible, "visible" },
            { StaticMapRequestParameters.Style, "style" },
            { StaticMapRequestParameters.Key, "key" }
        };
}