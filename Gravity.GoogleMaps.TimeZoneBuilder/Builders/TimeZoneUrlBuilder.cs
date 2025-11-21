using System.Globalization;

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
public class TimeZoneUrlBuilder
{
    // Fields

    private readonly Dictionary<TimeZoneQueryParameter, string> _requestParameters = new();
    private TimeZoneBuilderOptions _options = new();
    
    // Methods

    /// <summary>
    /// Add the location parameter to the query using latitude/longitude.
    /// </summary>
    /// <param name="latitude">The latitude used to retrieve timezone</param>
    /// <param name="longitude">The longitude used to retrieve timezone.</param>
    /// <returns>The builder instance.</returns>
    /// <exception cref="InvalidOperationException">Thrown when latitude is not between -90 and 90</exception>
    /// <exception cref="InvalidOperationException">Thrown when longitude is not between -180 and 180</exception>
    /// <exception cref="InvalidOperationException">Thrown when you call this method twice on the same builder.</exception>
    public TimeZoneUrlBuilder AddLocation(double latitude, double longitude)
    {
        if (longitude is < -180 or > 180) throw new InvalidOperationException("Latitude must be between -90 and 90");
        if (latitude is < -90 or > 90) throw new InvalidOperationException("Longitude must be between -180 and 180");
        
        string location = $"{latitude.ToString(CultureInfo.InvariantCulture)},{longitude.ToString(CultureInfo.InvariantCulture)}";

        if (!_requestParameters.TryAdd(TimeZoneQueryParameter.Location, location)) throw new InvalidOperationException($"You can't call {nameof(AddLocation)} twice !");
            
        return this;
    }

    /// <summary>
    /// Add the timestamp parameter to the query using the provided <see cref="DateTimeOffset"/>.
    /// </summary>
    /// <param name="dateTime">The value used to compute and add the timestamp parameter.</param>
    /// <returns>The builder instance.</returns>
    /// <exception cref="InvalidOperationException">Thrown when you call this method twice on the same builder.</exception>
    public TimeZoneUrlBuilder AddTimeStamp(DateTimeOffset dateTime)
    {
        var timestamp = dateTime.ToUnixTimeSeconds().ToString(CultureInfo.InvariantCulture);
        
        if (!_requestParameters.TryAdd(TimeZoneQueryParameter.Timestamp, timestamp)) throw new InvalidOperationException($"You can't call {nameof(AddTimeStamp)} twice !");

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
    /// <exception cref="InvalidOperationException">Thrown when you call this method twice on the same builder.</exception>
    public TimeZoneUrlBuilder AddLanguage(string language)
    {
        if (string.IsNullOrEmpty(language)) throw new ArgumentNullException(language);

        if (language.Length > 2) throw new InvalidOperationException("Language must be in two letters");
        
        if (!_requestParameters.TryAdd(TimeZoneQueryParameter.Language, language)) throw new InvalidOperationException($"You can't call {nameof(AddLanguage)} twice !");

        return this;
    }

    /// <summary>
    /// Add the API key to the query.
    /// </summary>
    /// <param name="apiKey">You API Key.</param>
    /// <returns>The builder.</returns>
    public TimeZoneUrlBuilder AddKey(string apiKey)
    {
        if (string.IsNullOrEmpty(apiKey)) throw new ArgumentNullException(apiKey);
        
        if (!_requestParameters.TryAdd(TimeZoneQueryParameter.ApiKey, apiKey)) throw new InvalidOperationException($"You can't call {nameof(AddKey)} twice !");

        return this;
    }

    /// <summary>
    /// Add builder option to custom the result of the <see cref="Build"/> method.
    /// </summary>
    /// <param name="options">The builder options</param>
    /// <returns>The builder.</returns>
    public TimeZoneUrlBuilder WithOptions(TimeZoneBuilderOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        
        _options = options;

        return this;
    }

    /// <summary>
    /// Build the TimeZone url.
    /// </summary>
    /// <remarks>
    /// This method validates the parameters and builds the url.
    /// <br/>
    /// The url is, by default, url encoded. Use <see cref="WithOptions"/> to disable the url encoding.
    /// </remarks>
    /// <returns>The TimeZone url generated according to added parameters and build options.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the API Key is missing and the <see cref="TimeZoneBuilderOptions.DisableApiKeyCheck"/> is false.</exception>
    /// <exception cref="InvalidOperationException">Thrown when no parameter was added to the builder.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the location parameter is missing.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the timestamp parameter is missing.</exception>
    public string Build()
    {
        ValidateParameters();
        
        string url;

        if (_options.ReturnParametersOnly)
        {
            url = string.Empty;
        }
        else
        {
            url = ProjectConstants.TimeZoneBaseUrl + '?';
        }

        List<string> parts = [];

        foreach ((TimeZoneQueryParameter type, string value) in _requestParameters)
        {
            string parameter = value;
            
            if (!_options.DisableUrlEncoding)
            {
                parameter = Uri.EscapeDataString(parameter);
            }
            
            string paramName = RequestParameterNames.Map[type];

            parts.Add($"{paramName}={parameter}");
        }
        
        url += string.Join("&", parts);
        
        return url;
    }

    private void ValidateParameters()
    {
        if (_requestParameters.Count == 0) throw new InvalidOperationException("No parameters have been added");
        if (!_requestParameters.ContainsKey(TimeZoneQueryParameter.Location)) throw new InvalidOperationException($"Location parameter is mandatory, use {nameof(AddLocation)} method.");
        if (!_requestParameters.ContainsKey(TimeZoneQueryParameter.Timestamp))  throw new InvalidOperationException($"Timestamp parameter is mandatory, use {nameof(AddTimeStamp)} method.");

        if (_options.DisableApiKeyCheck) return;
        
        if (!_requestParameters.ContainsKey(TimeZoneQueryParameter.ApiKey))  throw new InvalidOperationException($"Api Key must be set. To build your url without use the {nameof(WithOptions)} method to configure builder. Else, use {nameof(AddKey)} method.");
    }
}