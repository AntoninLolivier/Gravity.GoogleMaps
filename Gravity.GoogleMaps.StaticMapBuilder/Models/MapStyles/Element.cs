namespace Gravity.GoogleMaps.StaticMapBuilder.Models.MapStyles;

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

    public override string ToString() => _value;
}
