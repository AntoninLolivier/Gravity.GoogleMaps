namespace Gravity.GoogleMaps.StaticMapBuilder.Builders;

/// <summary>
/// Represents configuration options that control how the <see cref="StaticMapsUrlBuilder"/>
/// generates a Google Static Maps request URL.
/// </summary>
public record StaticMapBuilderOptions
{
    /// <value>
    /// When set to <c>true</c>, disables enforcement of the API key requirement.
    /// </value>
    /// <remarks>
    /// Normally, the builder validates that an API key is provided before generating the final URL.
    /// Enable this option only when you explicitly need to bypass this validation, such as when
    /// generating a mock or placeholder URL for testing purposes.
    /// </remarks>
    public bool DisableApiKeyCheck { get; init; }
    
    /// <summary>
    /// Instructs the builder to return only the query parameters,
    /// without including the full base address
    /// (e.g. <c>"center=Paris&amp;zoom=10&amp;size=1x1&amp;key=YOUR_API_KEY"</c>).
    /// </summary>
    /// <remarks>
    /// This is useful when working with a preconfigured <see cref="HttpClient"/> that already contains
    /// the Google Static Maps endpoint as its <see cref="HttpClient.BaseAddress"/>, or when composing
    /// the final URL externally.  
    /// It ensures that the builder does not prepend the protocol and base path.
    /// </remarks>
    public bool ReturnParametersOnly { get; init; }
    
    /// <value>
    /// When set to <c>true</c>, disables URL encoding of parameter values.
    /// </value>
    /// <remarks>
    /// URL encoding is enabled by default and recommended in accordance with 
    /// the <see href="https://developers.google.com/maps/url-encoding">official Google Maps encoding guidelines</see>.  
    /// Disabling encoding should be done with caution and only when you are certain
    /// that the values are already safely encoded or when constructing URLs for debugging.
    /// </remarks>
    public bool DisableUrlEncoding { get; init; }
    
    /// <value>
    /// When set to <c>true</c>, instructs the builder to generate URLs using the HTTP protocol
    /// instead of HTTPS.
    /// </value>
    /// <remarks>
    /// HTTPS is used by default for security and compliance reasons.  
    /// This option should be enabled only for development scenarios,
    /// legacy systems, or controlled network environments where HTTP is required.
    /// </remarks>
    public bool UseHttp { get; init; }
}
