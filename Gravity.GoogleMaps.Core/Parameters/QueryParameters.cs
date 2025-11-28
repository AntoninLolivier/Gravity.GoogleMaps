using FluentValidation;
using FluentValidation.Results;

namespace Gravity.GoogleMaps.Core.Parameters;

public abstract class QueryParameters
{
    protected readonly List<IQueryParameter> Parameters = [];

    public List<string> GetQueryValues(bool disableUrlEncoding) => Parameters.Select(p =>
        disableUrlEncoding
            ? $"{p.Key}={p.Value}"
            : $"{p.Key}={Uri.EscapeDataString(p.Value.Value)}").ToList();

    public void Add(IQueryParameter parameter)
    {
        Parameters.Add(parameter);
    }

    public virtual ValidationResult Validate(bool disableApiKeyCheck)
    {
        List<ValidationResult> results = [];
        
        results.AddRange(Parameters.Select(queryParameter =>
        {
            if (queryParameter is ApiKeyQueryParameter && disableApiKeyCheck) return new ValidationResult();
            
            return queryParameter.Validate();
        }));
        
        results.Add(new QueryParameterValidator().Validate(this));
        
        return new ValidationResult(results);
    }

    private class QueryParameterValidator : AbstractValidator<QueryParameters>
    {
        public QueryParameterValidator()
        {
            List<string> duplicatesNames = [];

            RuleFor(x => x.Parameters)
                .NotEmpty()
                .WithMessage(ValidationMessages.NoParametersAddedExceptionMessage);
            
            RuleFor(x => x.Parameters)
                .Must(parameters =>
                {
                    var duplicates = parameters
                        .Where(p => p.IsUnique)
                        .GroupBy(p => p.GetType())
                        .Where(g => g.Count() > 1)
                        .ToList();
                    
                    duplicatesNames.AddRange(duplicates.Select(g => g.Key.Name));

                    return duplicates.Count == 0;
                })
                .WithMessage($"Duplicate unique parameters detected: {string.Join(", ", duplicatesNames)}");
        }
    }
}