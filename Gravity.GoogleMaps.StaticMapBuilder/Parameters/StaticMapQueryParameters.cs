using FluentValidation;
using FluentValidation.Results;

namespace Gravity.GoogleMaps.StaticMapBuilder.Parameters;

public class StaticMapQueryParameters : QueryParameters
{
    // Properties

    public int LocationsCount { get; set; } = 0;
    
    public int LocationMarkersCount { get; set; } = 0;
    
    public int LocationPointForPathCount { get; set; } = 0;
    
    public List<string> MarkerIconUrls { get; } = [];
    
    // Methods
    
    public override ValidationResult Validate(bool disableApiKeyCheck)
    {
        var globalValidationsResult = new StaticMapQueryParametersValidator(disableApiKeyCheck).Validate(this);
        var singleValidationsResult = base.Validate(disableApiKeyCheck);

        return new ValidationResult([globalValidationsResult, singleValidationsResult]);
    }

    private class StaticMapQueryParametersValidator : AbstractValidator<StaticMapQueryParameters>
    {
        public StaticMapQueryParametersValidator(bool disableApiKeyCheck)
        {
            RuleFor(x => x.Parameters.Count)
                .GreaterThan(0)
                .WithMessage(ExceptionMessages.NoParametersAddedExceptionMessage);

            RuleFor(x => x.Parameters)
                .Must(parameters => parameters.Any(p => p is SizeQueryParameter))
                .WithMessage(ExceptionMessages.ParametersMissingExceptionMessages.SizeParameterMissingMessage);

            RuleFor(x => x.Parameters)
                .Must(parameters =>
                {
                    bool containMapId = parameters.Any(p => p is MapIdQueryParameter);
                    bool containStyle = parameters.Any(p => p is MapStyleQueryParameter);

                    if (containStyle && containMapId) return false;

                    return true;
                })
                .WithMessage(ExceptionMessages.UrlParametersExceptionMessages.CombineMapIdAndMapStyleExceptionMessage);

            RuleFor(x => x.Parameters)
                .Must(parameters =>
                {
                    bool hasCenter = parameters.Any(p => p is CenterQueryParameter);
                    bool hasZoom = parameters.Any(p => p is ZoomQueryParameter);
                    bool hasMarker = parameters.Any(p => p is MarkerQueryParameter);
                    bool hasPath = parameters.Any(p => p is PathQueryParameter);
                    bool hasViewport = parameters.Any(p => p is ViewportsQueryParameter);

                    bool requiresCenterOrZoom = !(hasCenter || hasZoom || hasMarker || hasPath || hasViewport);

                    if (requiresCenterOrZoom && !hasCenter && !hasZoom) return false;

                    return true;
                })
                .WithMessage(ExceptionMessages.ParametersMissingExceptionMessages.CenterParameterMissingMessage);

            RuleFor(x => x.LocationMarkersCount)
                .LessThanOrEqualTo(ApiLimits.LocationMarkersCountLimit)
                .WithMessage(ExceptionMessages.UrlParametersExceptionMessages.TooMuchLocationMarkersExceptionMessage);

            RuleFor(x => x.MarkerIconUrls.Distinct().Count())
                .LessThanOrEqualTo(ApiLimits.CustomMakerIconsCountLimit)
                .WithMessage(ExceptionMessages.UrlParametersExceptionMessages.TooMuchDistinctCustomMarkerIcons);
            
            RuleFor(parameters => parameters.LocationPointForPathCount)
                .LessThanOrEqualTo(ApiLimits.LocationPointsForPathCountLimit)
                .WithMessage(ExceptionMessages.UrlParametersExceptionMessages.TooMuchLocationPointsForPathsExceptionMessage);

            RuleFor(parameters => parameters.LocationsCount)
                .LessThanOrEqualTo(ApiLimits.LocationsCountLimit)
                .WithMessage(ExceptionMessages.UrlParametersExceptionMessages.TooMuchLocationsExceptionMessage);

            if (!disableApiKeyCheck)
            {
                RuleFor(x => x.Parameters)
                    .Must(parameters => parameters.Any(p => p is ApiKeyQueryParameter))
                    .WithMessage(ExceptionMessages.ParametersMissingExceptionMessages.ApiKeyParameterMissingMessage);
            }
        }
    }
}