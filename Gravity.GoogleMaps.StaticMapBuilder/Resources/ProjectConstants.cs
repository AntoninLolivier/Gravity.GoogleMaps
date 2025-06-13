namespace Gravity.GoogleMaps.StaticMapBuilder.Resources;

internal static class ProjectConstants
{
    internal const string StaticMapBaseUrlHttps = "https://maps.googleapis.com/maps/api/staticmap";
    internal const string StaticMapBaseUrlHttp = "http://maps.googleapis.com/maps/api/staticmap";

    internal const int StaticMapsApiUrlMaxSize = 16_384;
    internal const int LocationsCountLimit = 3;
    internal const int LocationMarkersCountLimit = 15;
    internal const int LocationPointsForPathCountLimit = 15;
    internal const int MapIdMaxLength = 16;
    internal const int CustomMakerIconsCountLimit = 5;

    internal static class StaticMapDocumentationLinks
    {
        internal const string OverviewLink = "https://developers.google.com/maps/documentation/maps-static/overview";
        internal const string StartLink = "https://developers.google.com/maps/documentation/maps-static/start";
        internal const string UrlSizeRestrictionLink = "https://developers.google.com/maps/documentation/maps-static/start#url-size-restriction";

        internal static class SectionLinks
        {
            internal const string LocationParameters = "https://developers.google.com/maps/documentation/maps-static/start#location";
            internal const string MapParameters = "https://developers.google.com/maps/documentation/maps-static/start#map-parameters";
            internal const string FeaturesParameters = "https://developers.google.com/maps/documentation/maps-static/start#feature-parameters";
            internal const string KeyAndSignature = "https://developers.google.com/maps/documentation/maps-static/start#key-and-signature-parameters";
            internal const string ZoomLevels = "https://developers.google.com/maps/documentation/maps-static/start?#Zoomlevels";
            internal const string Locations = "https://developers.google.com/maps/documentation/maps-static/start#Locations";
            internal const string MarkerLocations = "https://developers.google.com/maps/documentation/maps-static/start#MarkerLocations";
            internal const string MarkerStyles = "https://developers.google.com/maps/documentation/maps-static/start?#MarkerStyles";
            internal const string PathStyles = "https://developers.google.com/maps/documentation/maps-static/start#PathStyles";
            internal const string EncodedPolylines = "https://developers.google.com/maps/documentation/maps-static/start#EncodedPolylines";
            internal const string CustomIcons = "https://developers.google.com/maps/documentation/maps-static/start#CustomIcons";
        }

        internal static class MapStylingDocumentationLinks
        {
            internal const string MapStyling = "https://developers.google.com/maps/documentation/maps-static/styling";
            internal const string StyleRules = "https://developers.google.com/maps/documentation/maps-static/styling#style-rules";
        }
    }

    internal static class OtherLinks
    {
        internal const string RegionCodesList = "https://developers.google.com/maps/coverage#countryregion-coverage-for-core-mapping-features";
    }
}