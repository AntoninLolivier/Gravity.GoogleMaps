namespace Gravity.GoogleMaps.TimeZoneBuilder.Builders;

/// <summary>
/// The main class used to build the TimeZone url.
/// </summary>
/// <example>
/// <code>
/// string url = new TimeZoneUrlBuilder()
///        .AddLocation(47.3943556, 0.6123703)
///        .AddTimeStamp(DateTimeOffset.UtcNow)
///        .AddLanguage("fr")
///        .AddKey("YOUR_API_KEY")
///        .Build();
/// </code>
/// </example>
public class TimeZoneUrlBuilder() : GoogleMapsBuilder<TimeZoneUrlBuilder, TimeZoneQueryParameters, TimeZoneBuilderOptions>(new TimeZoneBuilderOptions())
{
    // Properties
    
    /// <inheritdoc />
    protected override TimeZoneQueryParameters Parameters { get; } = new();
    
    // Methods

    /// <inheritdoc />
    protected override string GetBaseUrl() => ApiEndpoints.TimeZoneBaseUrl;

    /// <summary>
    /// Add the location parameter to the query using latitude/longitude.
    /// </summary>
    /// <param name="latitude">The latitude used to retrieve timezone</param>
    /// <param name="longitude">The longitude used to retrieve timezone.</param>
    /// <returns>The builder instance.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when latitude is not between -90 and 90</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when longitude is not between -180 and 180</exception>
    public TimeZoneUrlBuilder AddLocation(double latitude, double longitude)
    {
        if (latitude is < -90 or > 90) throw new ArgumentOutOfRangeException(nameof(latitude), message: ExceptionMessages.MalformedParametersExceptionMessages.LatitudeOutOfRangeMessage);
        if (longitude is < -180 or > 180) throw new ArgumentOutOfRangeException(nameof(longitude), message: ExceptionMessages.MalformedParametersExceptionMessages.LongitudeOutOfRangeMessage);
        
        Parameters.Add(new LocationQueryParameter(latitude, longitude));
            
        return this;
    }

    /// <summary>
    /// Add the timestamp parameter to the query using the provided <see cref="DateTimeOffset"/>.
    /// </summary>
    /// <param name="dateTime">The value used to compute and add the timestamp parameter.</param>
    /// <returns>The builder instance.</returns>
    public TimeZoneUrlBuilder AddTimeStamp(DateTimeOffset dateTime)
    {
        Parameters.Add(new TimestampQueryParameter(dateTime));

        return this;
    }

    /// <summary>
    /// Add the language parameter to the query using the provided string (Must be two letter long)
    /// </summary>
    /// <remarks>
    /// See <see href="https://developers.google.com/maps/documentation/maps-static/start#map-parameters">official documentation</see> for details.
    /// </remarks>
    /// <param name="language">The language to add as 2 letter representation (i.e "fr", "en", etc.)</param>
    /// <returns>The builder instance.</returns>
    /// <exception cref="ArgumentNullException">Throw if <paramref name="language"/> is null or empty.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="language"/> is not 2 letters long.</exception>
    public TimeZoneUrlBuilder AddLanguage(string language)
    {
        if (string.IsNullOrEmpty(language) || string.IsNullOrWhiteSpace(language)) throw new ArgumentNullException(nameof(language));
        if (language.Length != 2) throw new ArgumentException("Language must be in two letters", nameof(language));
        
        Parameters.Add(new LanguageQueryParameter(language));

        return this;
    }
}