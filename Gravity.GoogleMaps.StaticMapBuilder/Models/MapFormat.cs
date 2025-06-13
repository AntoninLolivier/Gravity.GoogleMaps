using Gravity.GoogleMaps.StaticMapBuilder.Options;

namespace Gravity.GoogleMaps.StaticMapBuilder.Models;

/// <summary>
/// Map format.
/// </summary>
/// <remarks>
/// See <see href="https://developers.google.com/maps/documentation/maps-static/start#map-parameters">official documentation</see>
/// for details.
/// <br/>
/// Use the <see cref="MapFormats"/> to get the available formats.
/// </remarks>
public sealed class MapFormat 
{
    // Properties
    
    private readonly string _value;
    
    // Constructor
    
    internal MapFormat(string value)
    {
        _value = value;
    }

    // Methods

    /// <inheritdoc />
    public override string ToString() => _value;
}