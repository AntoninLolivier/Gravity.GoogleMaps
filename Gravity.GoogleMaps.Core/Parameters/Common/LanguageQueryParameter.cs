using FluentValidation;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace Gravity.GoogleMaps.Core.Parameters.Common;

public class LanguageQueryParameter(string value) : IQueryParameter
{
    public string Key { get; } = "language";
    
    public bool IsUnique { get; } = true;

    public ParameterValue Value { get; } = new SimpleParameterValue(value);
    
    public ValidationResult Validate() => new LanguageQueryParameterValidator().Validate(value);
}

public class LanguageQueryParameterValidator : AbstractValidator<string>
{
    public LanguageQueryParameterValidator()
    {
        RuleFor(x => x)
            .NotEmpty()
            .NotNull()
            .Length(2)
            .WithMessage("Language must be in two letters");
    }
}