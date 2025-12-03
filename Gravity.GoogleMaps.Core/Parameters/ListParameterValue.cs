namespace Gravity.GoogleMaps.Core.Parameters;

/// <summary>
/// A type of parameter that contain a list of values separated by the <paramref name="Separator"/>
/// </summary>
/// <param name="Values">The values of the query parameter.</param>
/// <param name="Separator">The character that split the values in <paramref name="Values"/>.</param>
public record ListParameterValue(IEnumerable<string> Values, char Separator = '|') : ParameterValue
{
    /// <inheritdoc />
    public override string Value => string.Join(Separator, Values);
}