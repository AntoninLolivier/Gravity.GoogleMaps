namespace Gravity.GoogleMaps.StaticMapBuilder.Models.MapStyles;

public readonly record struct Feature
{
    // Properties
    
    public string Value { get; }
    
    // Constructor
    
    internal Feature(string Value)
    {
        this.Value = Value;
    }
    
    // Methods

    public override string ToString() => Value;
}