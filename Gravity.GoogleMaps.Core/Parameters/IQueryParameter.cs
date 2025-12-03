namespace Gravity.GoogleMaps.Core.Parameters;

/// <summary>
/// An interface to define a query parameter of a query builder.
/// </summary>
public interface IQueryParameter
{
    /// <value>
    /// The key of the query parameter, for exemple "key" for api key etc
    /// </value>
    string Key { get; }
    
    /// <value>
    /// A flag indicating if the parameter should appear only once per request.
    /// </value>
    bool IsUnique { get; }
    
    /// <summary>
    /// The value of the parameter, as a strong typed object.
    /// </summary>
    ParameterValue Value { get; }
}