namespace Gravity.GoogleMaps.StaticMapBuilder.Builders;

public record StaticMapBuilderOptions
{
    /// <value>
    /// Disable the api key check.
    /// </value>
    /// <remarks>
    /// Disable the api key check allowing to have a "raw" request url.
    /// </remarks>
    public bool DisableApiKeyCheck { get; init; }
    
    /// <summary>
    /// Instructs the builder to return only the query parameters,
    /// without including the full base address (i.e : "center=Paris&amp;zoom=10&amp;size=1x1&amp;key=YOUR_API_KEY")
    /// </summary>
    /// <remarks>
    /// This is useful when using a preconfigured <see cref="HttpClient"/> that already includes
    /// the Google Static Maps base address. It prevents overriding the base URI and allows clean 
    /// composition of the full request URL using only the relevant parameters.
    /// </remarks>
    public bool ReturnParametersOnly { get; init; }
    
    /// <value>
    /// Disable the url encoding. The url is, by default, url encoded.
    /// </value>
    /// <remarks>
    /// See <see href="https://developers.google.com/maps/url-encoding">official documentation</see> for details.
    /// </remarks>
    public bool DisableUrlEncoding { get; init; }
    
    /// <value>
    /// Use the HTTP protocol instead of HTTPS.
    /// </value>
    public bool UseHttp { get; init; }
}