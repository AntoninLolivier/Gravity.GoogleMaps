namespace Gravity.GoogleMaps.TimeZoneBuilder.ApiSpecs;

internal class TimeZoneParameterNames
{
    internal static readonly IReadOnlyDictionary<TimeZoneQueryParameter, string> Map =
        new Dictionary<TimeZoneQueryParameter, string>
        {
            { TimeZoneQueryParameter.Location, "location" },
            { TimeZoneQueryParameter.Timestamp, "timestamp" },
            { TimeZoneQueryParameter.Language, "language" },
            { TimeZoneQueryParameter.ApiKey, "key" }
        };
}