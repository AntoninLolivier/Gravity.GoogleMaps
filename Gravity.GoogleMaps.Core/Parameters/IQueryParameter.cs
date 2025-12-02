namespace Gravity.GoogleMaps.Core.Parameters;

public interface IQueryParameter
{
    string Key { get; }
    
    bool IsUnique { get; }
    
    ParameterValue Value { get; }
}