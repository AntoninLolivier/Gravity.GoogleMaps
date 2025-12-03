namespace Gravity.GoogleMaps.Core.Parameters.Common;

/// <summary>
/// The language parameter of the query.
/// </summary>
/// <param name="value">The value of the language query parameter.</param>
public class LanguageQueryParameter(string value) : IQueryParameter
{
    /// <inheritdoc />
    public string Key => "language";

    /// <inheritdoc />
    public bool IsUnique => true;

    /// <inheritdoc />
    public ParameterValue Value { get; } = new SimpleParameterValue(value);
}