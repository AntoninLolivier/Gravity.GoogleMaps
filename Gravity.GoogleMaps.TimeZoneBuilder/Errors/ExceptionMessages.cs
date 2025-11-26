namespace Gravity.GoogleMaps.TimeZoneBuilder.Errors;

internal static class ExceptionMessages
{
    internal const string NoParametersAddedExceptionMessage = "No parameters have been added";
    internal const string ParameterCanOnlyBeAddedOnceExceptionMessage = "The {0} parameter can only be added once";
    
    internal static class ParametersMissingExceptionMessages
    {
        internal const string LocationParameterMissingMessage = $"{nameof(TimeZoneQueryParameter.Location)} parameter is missing, use {nameof(TimeZoneUrlBuilder.AddLocation)} method.";
        internal const string TimestampParameterMissingMessage = $"{nameof(TimeZoneQueryParameter.Timestamp)} parameter is missing, use {nameof(TimeZoneUrlBuilder.AddTimeStamp)} method.";
        internal const string ApiKeyParameterMissingMessage = $"{nameof(TimeZoneQueryParameter.ApiKey)} parameter is missing, use {nameof(TimeZoneUrlBuilder.AddKey)} method or disbable this error using the {nameof(TimeZoneUrlBuilder.WithOptions)} method to configure builder.";
    }
    
    internal static class MalformedParametersExceptionMessages
    {
        internal const string LatitudeOutOfRangeMessage = "Latitude must be between -90 and 90";
        internal const string LongitudeOutOfRangeMessage = "Longitude must be between -180 and 180";
    }
}