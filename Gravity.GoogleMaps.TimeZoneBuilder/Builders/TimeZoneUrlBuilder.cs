using System.Globalization;

namespace Gravity.GoogleMaps.TimeZoneBuilder.Builders;

public class TimeZoneUrlBuilder
{
    // Fields

    private readonly Dictionary<TimeZoneQueryParameter, string> _requestParameters = new();
    private TimeZoneBuilderOptions _options = new();
    
    // Methods

    public TimeZoneUrlBuilder AddLocation(double latitude, double longitude)
    {
        if (longitude is < -180 or > 180) throw new InvalidOperationException("Latitude must be between -90 and 90");
        if (latitude is < -90 or > 90) throw new InvalidOperationException("Longitude must be between -180 and 180");
        
        string location = $"{latitude.ToString(CultureInfo.InvariantCulture)},{longitude.ToString(CultureInfo.InvariantCulture)}";

        if (!_requestParameters.TryAdd(TimeZoneQueryParameter.Location, location)) throw new InvalidOperationException($"You can't call {nameof(AddLocation)} twice !");
            
        return this;
    }

    public TimeZoneUrlBuilder AddTimeStamp(DateTimeOffset dateTime)
    {
        var timestamp = dateTime.ToUnixTimeSeconds().ToString(CultureInfo.InvariantCulture);
        
        if (!_requestParameters.TryAdd(TimeZoneQueryParameter.Timestamp, timestamp)) throw new InvalidOperationException($"You can't call {nameof(AddTimeStamp)} twice !");

        return this;
    }

    public TimeZoneUrlBuilder AddLanguage(string language)
    {
        if (string.IsNullOrEmpty(language)) throw new ArgumentNullException(language);

        if (language.Length > 2) throw new InvalidOperationException("Language must be in two letters");
        
        if (!_requestParameters.TryAdd(TimeZoneQueryParameter.Language, language)) throw new InvalidOperationException($"You can't call {nameof(AddLanguage)} twice !");

        return this;
    }

    public TimeZoneUrlBuilder AddKey(string apiKey)
    {
        if (string.IsNullOrEmpty(apiKey)) throw new ArgumentNullException(apiKey);
        
        if (!_requestParameters.TryAdd(TimeZoneQueryParameter.ApiKey, apiKey)) throw new InvalidOperationException($"You can't call {nameof(AddKey)} twice !");

        return this;
    }

    public TimeZoneUrlBuilder WithOptions(TimeZoneBuilderOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        
        _options = options;

        return this;
    }

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