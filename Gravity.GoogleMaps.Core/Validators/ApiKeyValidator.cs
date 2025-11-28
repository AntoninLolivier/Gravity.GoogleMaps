using FluentValidation;

namespace Gravity.GoogleMaps.Core.Validators;

public class ApiKeyValidator : AbstractValidator<string>
{
    public ApiKeyValidator()
    {
        RuleFor(x => x)
            .NotEmpty()
            .NotNull();
    }
}