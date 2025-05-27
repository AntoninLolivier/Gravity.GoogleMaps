using Gravity.GoogleMaps.StaticMapBuilder.Options;

namespace Gravity.GoogleMaps.StaticMapBuilder.Models.MapStyles;

/// <summary>
/// The element of a <see cref="MapStyle"/>.
/// </summary>
/// <remarks>
/// See <see href="https://developers.google.com/maps/documentation/maps-static/styling#elements">official documentation</see>
/// for details.
/// <br/>
/// Use the <see cref="Elements"/> to get the available elements.
/// </remarks>
public readonly record struct Element
{
    // Fields

    private readonly string _value;

    // Constructor
    
    internal Element(string value)
    {
        _value = value;
    }
    
    // Methods

    /// <inheritdoc />
    public override string ToString() => _value;
}
