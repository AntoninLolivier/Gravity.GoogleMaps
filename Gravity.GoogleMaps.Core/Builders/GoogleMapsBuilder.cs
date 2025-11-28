using ValidationException = FluentValidation.ValidationException;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace Gravity.GoogleMaps.Core.Builders;

public abstract class GoogleMapsBuilder<TParameters, TOption>
    where TParameters : QueryParameters
    where TOption : BuilderOptions
{
    private TOption _options;

    protected GoogleMapsBuilder(TOption options)
    {
        _options = options;
    }

    protected abstract string GetBaseUrl();

    protected abstract TParameters Parameters { get; set; }

    public GoogleMapsBuilder<TParameters, TOption> WithOptions(TOption options)
    {
        ArgumentNullException.ThrowIfNull(options);
        
        _options = options;

        return this;
    }

    public GoogleMapsBuilder<TParameters, TOption> AddKey(string apiKey)
    {
        Parameters.Add(new ApiKeyQueryParameter(apiKey));

        return this;
    }

    public virtual string Build()
    {
        ValidationResult result = Parameters.Validate(_options.DisableApiKeyCheck);
        
        if (!result.IsValid) throw new InvalidOperationException(result.ToString());
        
        string url;

        if (_options.ReturnParametersOnly)
        {
            url = string.Empty;
        }
        else
        {
            url = GetBaseUrl() + '?';
        }

        string queryString = string.Join('&', Parameters.GetQueryValues(_options.DisableUrlEncoding));
        
        url +=queryString;
        
        return url;
    }
}