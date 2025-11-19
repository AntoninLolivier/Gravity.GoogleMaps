namespace Gravity.GoogleMaps.StaticMapBuilder.Models.MapStyles;

/// <summary>
/// The feature of a <see cref="MapStyle"/>.
/// </summary>
/// <remarks>
/// See <see href="https://developers.google.com/maps/documentation/maps-static/styling#features">official documentation</see>
/// for details.
/// <br/>
/// Use the <see cref="Features"/> to get the available features.
/// </remarks>
public readonly record struct Feature
{
    // Fields

    private readonly string _value;
    
    // Constructor
    
    internal Feature(string value)
    {
        _value = value;
    }

    // Methods

    /// <inheritdoc />
    public override string ToString() => _value;
}