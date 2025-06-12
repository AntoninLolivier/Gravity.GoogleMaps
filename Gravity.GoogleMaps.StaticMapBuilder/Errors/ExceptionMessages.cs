namespace Gravity.GoogleMaps.StaticMapBuilder.Errors;

internal static class ExceptionMessages
{
    internal const string LabelNotSupportedExceptionMessage = "Label is not supported for Tiny or Small markers";
    internal const string NoParametersAddedExceptionMessage = "No parameters have been added";
    internal const string ParameterCanOnlyBeAddedOnceExceptionMessage = "The {0} parameter can only be added once";
    internal static readonly string StaticMapUrlExceededLengthExceptionMessage = $"Static map url exceeded max allowed length of {ProjectConstants.StaticMapsApiUrlMaxSize}, see" + ProjectConstants.StaticMapDocumentationLinks.UrlSizeRestrictionLink + " for more information";

    internal static class ParametersMissingExceptionMessages
    {
        internal const string CenterParameterMissingMessage = $"{nameof(StaticMapRequestParameters.Center)} parameter is mandatory if no markers, path or viewports elements are added, see : " + ProjectConstants.StaticMapDocumentationLinks.SectionLinks.LocationParameters + " for more information";
        internal const string SizeParameterMissingMessage = "Size parameter is mandatory, see : " + ProjectConstants.StaticMapDocumentationLinks.SectionLinks.MapParameters + " for more information";
        internal const string ZoomParameterMissingMessage = $"{nameof(StaticMapRequestParameters.Zoom)} parameter is mandatory if no markers are added, see : " + ProjectConstants.StaticMapDocumentationLinks.SectionLinks.LocationParameters + " for more information";
        internal const string ApiKeyParameterMissingMessage = "Key parameter is missing";
    }

    internal static class MalformedParametersExceptionMessages
    {
        internal const string LanguageMustBeInTwoLettersExceptionMessage = "Language must be in two letters";
        internal const string RegionMustBeInTwoLettersExceptionMessage = $"Region must be in two letters. See {ProjectConstants.OtherLinks.RegionCodesList} to see avaiable regions";
        internal static readonly string MapIdMustBeSixteenCharactersMax = $"Map id must be {ProjectConstants.MapIdMaxLength} characters max";
        internal const string LatitudeOutOfRangeMessage = "Latitude must be between -90 and 90";
        internal const string LongitudeOutOfRangeMessage = "Longitude must be between -180 and 180";
        internal const string LightnessOutOfRangeExceptionMessage = $"Lightness must be between -100 and 100, see {ProjectConstants.StaticMapDocumentationLinks.MapStylingDocumentationLinks.StyleRules}";
        internal const string SaturationOutOfRangeExceptionMessage = $"Saturation must be between -100 and 100, see {ProjectConstants.StaticMapDocumentationLinks.MapStylingDocumentationLinks.StyleRules}";
        internal const string GammaOutOfRangeExceptionMessage = $"Gamma must be between 0.01 and 10.0, see {ProjectConstants.StaticMapDocumentationLinks.MapStylingDocumentationLinks.StyleRules}";
        internal const string AlphaCannotBeSetForMarkers = $"Alpha cannot be set for markers !, see {ProjectConstants.StaticMapDocumentationLinks.SectionLinks.MarkerStyles}";
        internal const string MarkerAnchorXOutOfRangeMessage = $"Marker anchor X must be between 0 and the width of the icon (max 64 pixels), see {ProjectConstants.StaticMapDocumentationLinks.SectionLinks.CustomIcons}";
        internal const string MarkerAnchorYOutOfRangeMessage = $"Marker anchor Y must be between 0 and the height of the icon (max 64 pixels), see {ProjectConstants.StaticMapDocumentationLinks.SectionLinks.CustomIcons}";
    }
    
    internal static class UrlParametersExceptionMessages
    {
        internal const string PathCannotBeDefinedByPointsAndPolylineExceptionMessage = "Path cannot be defined by points and polyline simultaneously, see : " + ProjectConstants.StaticMapDocumentationLinks.SectionLinks.EncodedPolylines;
        internal const string PathNeedAtLeastTwoPointsExceptionMessage = "Path need at least two points ! Add 2 points or more to define a correct path !";
        internal const string CombineMapIdAndMapStyleExceptionMessage = $"Don't combine map style and map id. Choose either one or the other, see : {ProjectConstants.StaticMapDocumentationLinks.MapStylingDocumentationLinks.MapStyling}";
        internal static readonly string TooMuchLocationsExceptionMessage = $"Too much location ! Only {ProjectConstants.LocationsCountLimit} locations are allowed per map (the paths and markers are not included but the center is), see : {ProjectConstants.StaticMapDocumentationLinks.SectionLinks.Locations}";
        internal static readonly string TooMuchLocationMarkersExceptionMessage = $"Too much {nameof(LocationMarker)} ! Only {ProjectConstants.LocationMarkersCountLimit} {nameof(LocationMarker)} are allowed per map, see : {ProjectConstants.StaticMapDocumentationLinks.SectionLinks.MarkerLocations}";
        internal static readonly string TooMuchLocationPointsForPathsExceptionMessage = $"Too much geocoded locations in paths ! Only {ProjectConstants.LocationPointsForPathCountLimit} geocoded locations for paths are allowed per map.";
        public const string AnchorCanBeSetOnlyForCustomIconsExceptionMessage = "Anchor can be set only for custom icons !";
    }
}