using ValidationResult = FluentValidation.Results.ValidationResult;

namespace Gravity.GoogleMaps.Core.Builders;

public abstract class GoogleMapsBuilder<TBuilder, TParameters, TOption>
    where TBuilder : GoogleMapsBuilder<TBuilder, TParameters, TOption>
    where TParameters : QueryParameters
    where TOption : BuilderOptions
{
    protected TOption Options;

    protected GoogleMapsBuilder(TOption options)
    {
        Options = options;
    }

    protected abstract string GetBaseUrl();

    protected abstract TParameters Parameters { get; set; }

    public TBuilder WithOptions(TOption options)
    {
        ArgumentNullException.ThrowIfNull(options);
        
        Options = options;

        return (TBuilder)this;
    }

    public TBuilder AddKey(string apiKey)
    {
        Parameters.Add(new ApiKeyQueryParameter(apiKey));

        return (TBuilder)this;
    }

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