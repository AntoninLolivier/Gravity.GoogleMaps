namespace Gravity.GoogleMaps.StaticMapBuilder.Models;

public readonly record struct MapFormat
{
    // Properties
    
    private readonly string _value;
    
    // Constructor
    
    internal MapFormat(string value)
    {
        _value = value;
    }
    
    // Methods
    
    public override string ToString() => _value;
}