using FluentValidation;
using FluentValidation.Results;

namespace Gravity.GoogleMaps.TimeZoneBuilder.Parameters;

public class TimeZoneQueryParameters : QueryParameters
{
    public override ValidationResult Validate(bool disableApiKeyCheck)
    {
        var globalValidationsResult = new TimeZoneQueryParametersValidator(disableApiKeyCheck).Validate(this);
        var singleValidationsResult = base.Validate(disableApiKeyCheck);

        return new ValidationResult([globalValidationsResult, singleValidationsResult]);
    }

    private class TimeZoneQueryParametersValidator : AbstractValidator<TimeZoneQueryParameters>
    {
        public TimeZoneQueryParametersValidator(bool disableApiKeyCheck)
        {
            RuleFor(x => x.Parameters.Count)
                .GreaterThan(0)
                .WithMessage(ExceptionMessages.NoParametersAddedExceptionMessage);

            RuleFor(x => x.Parameters)
                .Must(parameters => parameters.Any(p => p is TimestampQueryParameter))
                .WithMessage(ExceptionMessages.ParametersMissingExceptionMessages.TimestampParameterMissingMessage);
            
            RuleFor(x => x.Parameters)
                .Must(parameters => parameters.Any(p => p is LocationQueryParameter))
                .WithMessage(ExceptionMessages.ParametersMissingExceptionMessages.LocationParameterMissingMessage);

            if (!disableApiKeyCheck)
            {
                RuleFor(x => x.Parameters)
                    .Must(parameters => parameters.Any(p => p is ApiKeyQueryParameter))
                    .WithMessage(ExceptionMessages.ParametersMissingExceptionMessages.ApiKeyParameterMissingMessage);
            }
        }
    }
}