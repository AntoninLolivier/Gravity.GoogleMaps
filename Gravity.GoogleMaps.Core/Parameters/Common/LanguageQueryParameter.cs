namespace Gravity.GoogleMaps.Core.Parameters.Common;

public class LanguageQueryParameter(string value) : IQueryParameter
{
    public string Key { get; } = "language";
    
    public bool IsUnique { get; } = true;

    public ParameterValue Value { get; } = new SimpleParameterValue(value);
}