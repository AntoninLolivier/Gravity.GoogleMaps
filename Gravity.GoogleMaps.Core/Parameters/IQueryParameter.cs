using ValidationResult = FluentValidation.Results.ValidationResult;

namespace Gravity.GoogleMaps.Core.Parameters;

public interface IQueryParameter
{
    string Key { get; }
    
    bool IsUnique { get; }
    
    ParameterValue Value { get; }

    ValidationResult Validate();
}