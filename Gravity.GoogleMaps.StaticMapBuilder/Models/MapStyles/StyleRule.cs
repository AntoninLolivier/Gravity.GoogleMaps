using System.Globalization;

namespace Gravity.GoogleMaps.StaticMapBuilder.Models.MapStyles;

/// <summary>
/// Represents a single style rule for a <see cref="MapStyle"/>.
/// </summary>
/// <remarks>
/// See <see href="https://developers.google.com/maps/documentation/maps-static/styling#style-rules">official documentation</see>
/// for details.
/// </remarks>
public record StyleRule
{
    // Backing Fields
    
    private readonly double _lightness;
    private readonly double _saturation;
    private readonly double _gamma;
    
    // Properties

    /// <summary>
    /// The lightness adjustment of a <see cref="StyleRule"/>.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the value is outside the range of -100 to 100.
    /// </exception>
    public double Lightness
    {
        get => _lightness;
        init
        {
            if (value is < -100 or > 100) throw new ArgumentOutOfRangeException(nameof(Lightness), ExceptionMessages.MalformedParametersExceptionMessages.LightnessOutOfRangeExceptionMessage);
            
            _lightness = value;
        }
    }

    /// <summary>
    /// The saturation adjustment of a <see cref="StyleRule"/>.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the value is outside the range of -100 to 100.
    /// </exception>
    public double Saturation
    {
        get => _saturation;
        init
        {
            if (value is < -100 or > 100) throw new ArgumentOutOfRangeException(nameof(Saturation), ExceptionMessages.MalformedParametersExceptionMessages.SaturationOutOfRangeExceptionMessage);
            
            _saturation = value;
        }
    }

    /// <summary>
    /// The gamma adjustment of a <see cref="StyleRule"/>.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the value is outside the range of 0.01 to 10.0.
    /// </exception>
    public double Gamma
    {
        get => _gamma;
        init
        {
            if (value is < 0.01 or > 10.0) throw new ArgumentOutOfRangeException(nameof(Gamma), ExceptionMessages.MalformedParametersExceptionMessages.GammaOutOfRangeExceptionMessage);
            _gamma = value;
        }
    }

    /// <summary>
    /// Specifies the hue adjustment of a <see cref="StyleRule"/>.
    /// </summary>
    public HexColor? Hue { get; }

    /// <summary>
    /// Indicates whether the lightness of the element is inverted in a <see cref="StyleRule"/>.
    /// </summary>
    public bool InvertLightness { get; }

    /// <summary>
    /// The visibility adjustment of a <see cref="StyleRule"/>.
    /// </summary>
    public Visibility Visibility { get; }

    /// <summary>
    /// Specifies the color to be applied as part of a <see cref="StyleRule"/>.
    /// </summary>
    public HexColor? Color { get; }

    /// <summary>
    /// The weight of the visual effect applied by the <see cref="StyleRule"/>.
    /// </summary>
    public int Weight { get; }

    // Constructor

    /// <summary>
    /// Represents a style rule, which defines visual modifications to specific map features or elements
    /// using parameters such as color, lightness, saturation, and more.
    /// </summary>
    /// <param name="Hue">Defines the base color using a hex color code.</param>
    /// <param name="Lightness">Adjusts the brightness level, with negative values decreasing brightness
    /// and positive values increasing it.</param>
    /// <param name="Saturation">Modifies the intensity of colors, where negative values desaturate
    /// and positive values enhance saturation.</param>
    /// <param name="Gamma">Controls the intensity of contrast, where default value is 1.0.</param>
    /// <param name="InvertLightness">Indicates whether the lightness of the style should be inverted.</param>
    /// <param name="Visibility">Determines the visibility of the map element. Possible values are
    /// On, Off, or Simplified.</param>
    /// <param name="Color">Specifies the color to apply using a hex color code.</param>
    /// <param name="Weight">Sets the weight/thickness for map features like roads, expressed as an integer.</param>
    public StyleRule(HexColor? Hue = null,
        double Lightness = 0d,
        double Saturation = 0d,
        double Gamma = 1d,
        bool InvertLightness = false,
        Visibility Visibility = Visibility.On,
        HexColor? Color = null,
        int Weight = 0)
    {
        this.Hue = Hue;
        this.InvertLightness = InvertLightness;
        this.Visibility = Visibility;
        this.Color = Color;
        this.Weight = Weight;
        this.Lightness = Lightness;
        this.Saturation = Saturation;
        this.Gamma = Gamma;
    }
    
    // Methods

    /// <inheritdoc />
    public override string ToString()
    {
        List<string> rules = []; 
        
        if (Hue is not null)
        {
            rules.Add($"hue:{Hue.Value.ToString()}");
        }

        if (Lightness is not 0d)
        {
            rules.Add($"lightness:{Lightness.ToString(CultureInfo.InvariantCulture)}");
        }
        
        if (Saturation is not 0d)
        {
            rules.Add($"saturation:{Saturation.ToString(CultureInfo.InvariantCulture)}");
        }
        
        if (Gamma is not 1d)
        {
            rules.Add($"gamma:{Gamma.ToString(CultureInfo.InvariantCulture)}");
        }

        if (InvertLightness)
        {
            rules.Add("invert_lightness:true");
        }
        
        if (Visibility is not Visibility.On)
        {
            rules.Add($"visibility:{Visibility.ToString().ToLower()}");
        }
        
        if (Color is not null)
        {
            rules.Add($"color:{Color.Value.ToString()}");
        }

        if (Weight != 0)
        {
            rules.Add($"weight:{Weight.ToString(CultureInfo.InvariantCulture)}");
        }

        return string.Join("|", rules);
    }
}