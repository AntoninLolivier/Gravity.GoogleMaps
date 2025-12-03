namespace Gravity.GoogleMaps.StaticMapBuilder.Builders;

/// <summary>
/// Represents configuration options that control how the <see cref="StaticMapsUrlBuilder"/>
/// generates a Google Static Maps request URL.
/// </summary>
public record StaticMapBuilderOptions : BuilderOptions
{
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
