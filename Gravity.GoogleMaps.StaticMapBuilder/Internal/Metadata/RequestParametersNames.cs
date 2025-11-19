namespace Gravity.GoogleMaps.StaticMapBuilder.Internal.Metadata;

internal static class RequestParameterNames
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