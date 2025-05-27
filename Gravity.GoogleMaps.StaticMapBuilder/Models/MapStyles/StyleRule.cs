using System.Globalization;

namespace Gravity.GoogleMaps.StaticMapBuilder.Models.MapStyles;

public record StyleRule
{
    // Backing Fields
    
    private readonly double _lightness;
    private readonly double _saturation;
    private readonly double _gamma;
    
    // Properties

    public double Lightness
    {
        get => _lightness;
        init
        {
            if (value is < -100 or > 100) throw new ArgumentOutOfRangeException(nameof(Lightness), ExceptionMessages.MalformedParametersExceptionMessages.LightnessOutOfRangeExceptionMessage);
            
            _lightness = value;
        }
    }

    public double Saturation
    {
        get => _saturation;
        init
        {
            if (value is < -100 or > 100) throw new ArgumentOutOfRangeException(nameof(Saturation), ExceptionMessages.MalformedParametersExceptionMessages.SaturationOutOfRangeExceptionMessage);
            
            _saturation = value;
        }
    }

    public double Gamma
    {
        get => _gamma;
        init
        {
            if (value is < 0.01 or > 10.0) throw new ArgumentOutOfRangeException(nameof(Gamma), ExceptionMessages.MalformedParametersExceptionMessages.GammaOutOfRangeExceptionMessage);
            _gamma = value;
        }
    }

    public HexColor? Hue { get; }
    
    public bool InvertLightness { get; }
    
    public Visibility Visibility { get; }
    
    public HexColor? Color { get; }
    
    public int Weight { get; }

    // Constructor

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