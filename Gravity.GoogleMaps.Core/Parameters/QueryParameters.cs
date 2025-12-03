using FluentValidation;
using FluentValidation.Results;

namespace Gravity.GoogleMaps.Core.Parameters;

/// <summary>
/// Base class for api-specific parameters.
/// </summary>
public abstract class QueryParameters
{
    /// <value>
    /// The parameters of the api.
    /// </value>
    protected readonly List<IQueryParameter> Parameters = [];

    /// <summary>
    /// Retrieve the formatted query parameters, ready to be added in the query (key=value)
    /// </summary>
    /// <param name="disableUrlEncoding">A flag indicating if the parameters values should be Url encoded.</param>
    /// <returns>The ready to be added query parameters values (key=value)</returns>
    public List<string> GetQueryValues(bool disableUrlEncoding) => Parameters.Select(p =>
        disableUrlEncoding
            ? $"{p.Key}={p.Value}"
            : $"{p.Key}={Uri.EscapeDataString(p.Value.Value)}").ToList();

    /// <summary>
    /// Add a parameters to the query.
    /// </summary>
    /// <param name="parameter">The parameter to add.</param>
    public void Add(IQueryParameter parameter)
    {
        Parameters.Add(parameter);
    }

    /// <summary>
    /// Validate globally the parameters (with dependency validation)
    /// </summary>
    /// <param name="disableApiKeyCheck">A flag indicating if the api key validation should be performed.</param>
    /// <returns>The result of the validation.</returns>
    public virtual ValidationResult Validate(bool disableApiKeyCheck)
    {
        List<ValidationResult> results =
        [
            new QueryParameterValidator().Validate(this)
        ];

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