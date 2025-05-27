namespace Gravity.GoogleMaps.StaticMapBuilder.Models.MapStyles;

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

    public override string ToString() => _value;
}