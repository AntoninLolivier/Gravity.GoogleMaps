using FluentValidation;

namespace Gravity.GoogleMaps.Core.Validators;

public class CoordinatesValidator : AbstractValidator<(double Longitude, double Latitude)>
{
    public CoordinatesValidator()
    {
        RuleFor(x => x.Longitude)
            .InclusiveBetween(-180, 180);
        
        RuleFor(x => x.Latitude)
            .InclusiveBetween(-180, 180);
    }
}