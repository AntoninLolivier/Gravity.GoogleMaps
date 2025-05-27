namespace Gravity.GoogleMaps.StaticMapBuilder.Models.MapStyles;

public readonly record struct Feature
{
    // Fields

    private readonly string _value;
    
    // Constructor
    
    internal Feature(string Value)
    {
        this._value = Value;
    }
    
    // Methods

    public override string ToString() => _value;
}