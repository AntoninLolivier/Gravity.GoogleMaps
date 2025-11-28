using FluentValidation.Results;
using Gravity.GoogleMaps.Core.Validators;

namespace Gravity.GoogleMaps.Core.Parameters.Common;

public class ApiKeyQueryParameter(string rawValue) : IQueryParameter
{
    public string Key => "key";
    
    public bool IsUnique => true;
    
    public ParameterValue Value { get; } = new SimpleParameterValue(rawValue);

    public ValidationResult Validate()
    {
        return new ApiKeyValidator().Validate(Key);
    }
}

