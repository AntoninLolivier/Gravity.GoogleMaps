using ValidationResult = FluentValidation.Results.ValidationResult;

namespace Gravity.GoogleMaps.Core.Builders;

/// <summary>
/// The base class for all the Google Maps api builders.
/// </summary>
/// <param name="options">The options that manage the building process.</param>
/// <typeparam name="TBuilder">The type of the builder</typeparam>
/// <typeparam name="TParameters">The type of the parameters of the builder.</typeparam>
/// <typeparam name="TOption">The type of the builder options.</typeparam>
public abstract class GoogleMapsBuilder<TBuilder, TParameters, TOption>(TOption options)
    where TBuilder : GoogleMapsBuilder<TBuilder, TParameters, TOption>
    where TParameters : QueryParameters
    where TOption : BuilderOptions
{
    // Properties
    
    /// <summary>
    /// The builder options that manage the building process.
    /// </summary>
    protected TOption Options = options;
    
    /// <summary>
    /// The object that contain the parameters of the query
    /// </summary>
    protected abstract TParameters Parameters { get; }
    
    // Methods

    /// <summary>
    /// Retrieve the base url of the query (static map, timezone, weather etc...)
    /// </summary>
    /// <returns>The base url of the query.</returns>
    protected abstract string GetBaseUrl();

    /// <summary>
    /// Manage options to customize the builder.
    /// </summary>
    /// <param name="options">The options that will change the building process.</param>
    /// <returns>The builder.</returns>
    public TBuilder WithOptions(TOption options)
    {
        ArgumentNullException.ThrowIfNull(options);
        
        Options = options;

        return (TBuilder)this;
    }

    /// <summary>
    /// Add the api key to the query.
    /// </summary>
    /// <param name="apiKey">Your api key.</param>
    /// <returns>The builder.</returns>
    public TBuilder AddKey(string apiKey)
    {
        Parameters.Add(new ApiKeyQueryParameter(apiKey));

        return (TBuilder)this;
    }

    /// <summary>
    /// Validate and build the final query according to the added parameters.
    /// </summary>
    /// <returns>The query string.</returns>
    /// <exception cref="InvalidOperationException">Thrown when a validation error occurs.</exception>
    public virtual string Build()
    {
        ValidationResult result = Parameters.Validate(Options.DisableApiKeyCheck);
        
        if (!result.IsValid) throw new InvalidOperationException(result.ToString());
        
        string url;

        if (Options.ReturnParametersOnly)
        {
            url = string.Empty;
        }
        else
        {
            url = GetBaseUrl() + '?';
        }

        string queryString = string.Join('&', Parameters.GetQueryValues(Options.DisableUrlEncoding));
        
        url +=queryString;
        
        return url;
    }
}