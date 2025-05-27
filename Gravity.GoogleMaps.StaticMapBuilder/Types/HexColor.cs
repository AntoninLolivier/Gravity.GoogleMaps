namespace Gravity.GoogleMaps.StaticMapBuilder.Types;

/// <summary>
/// Represents a HEX color used for specifying custom color values, primarily
/// formatted as 0xRRGGBB or 0xRRGGBBAA.
/// </summary>
/// <remarks>
/// - The HexColor struct allows flexible representation of both opaque and
/// semi-transparent HEX colors.
/// - Alpha transparency (AA) can be included as part of the HEX format.
/// - It performs validation to ensure proper formatting of the supplied
/// string value.
/// </remarks>
/// <example>
/// A valid HexColor value must follow the format 0xRRGGBB for fully opaque
/// colors or 0xRRGGBBAA for transparency.
/// </example>
public readonly struct HexColor
{
    // Fields

    private readonly string _value;
    
    // Properties
    
    internal bool IsAlphaSet { get; }
    
    // Constructors

    /// <summary>
    /// Initialize a new instance of the <see cref="HexColor"/> struct.
    /// </summary>
    /// <remarks>
    /// The color must be formatted as 0xRRGGBB or 0xRRGGBBAA.
    /// </remarks>
    /// <param name="value">The hexadecimal color as a string.</param>
    /// <exception cref="ArgumentException">Thrown when the <paramref name="value"/> is in the wrong format.</exception>
    public HexColor(string value)
    {
        if (!IsValidHex(value)) throw new ArgumentException("Invalid hex color format. Expected format: 0xRRGGBB or 0xRRGGBBAA.", nameof(value));
        if (value.Length is 10)
        {
            if (!string.Equals(value.Substring(8, 2).ToUpper(), "FF"))
            {
                IsAlphaSet = true;
            }
        }
        
        _value = value;
    }
    
    // Operators
    
    /// <summary>
    /// Implicitly convert a string to a HexColor.
    /// </summary>
    /// <param name="value">The hexadecimal color as a string.</param>
    /// <returns>An instance new instance of the <see cref="HexColor"/> struct.</returns>
    public static implicit operator HexColor(string value) => new(value);
    
    // Methods

    /// <inheritdoc />
    public override string ToString() => _value;

    private static bool IsValidHex(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return false;
        if (!input.StartsWith("0x", StringComparison.OrdinalIgnoreCase)) return false;
        string hex = input[2..];
        return hex.Length is 6 or 8 && int.TryParse(hex, System.Globalization.NumberStyles.HexNumber, null, out _);
    }
}