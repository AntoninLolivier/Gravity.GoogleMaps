namespace Gravity.GoogleMaps.StaticMapBuilder.Types;

public readonly struct HexColor
{
    // Properties
    
    public string Value { get; }
    
    internal bool IsAlphaSet { get; }
    
    // Constructors

    public HexColor(string value)
    {
        if (!IsValidHex(value)) throw new ArgumentException("Invalid hex color format. Expected format: 0xRRGGBB or 0xRRGGBBAA.", nameof(value));
        if (value.Length is 8) IsAlphaSet = true;
        
        Value = value;
    }
    
    // Operators
    
    public static implicit operator HexColor(string value) => new(value);
    
    // Methods

    public override string ToString() => Value;

    private static bool IsValidHex(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return false;
        if (!input.StartsWith("0x", StringComparison.OrdinalIgnoreCase)) return false;
        string hex = input[2..];
        return hex.Length is 6 or 8 && int.TryParse(hex, System.Globalization.NumberStyles.HexNumber, null, out _);
    }
}