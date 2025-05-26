namespace Gravity.GoogleMaps.StaticMapBuilder.Models.MapStyles;

public record StyleRule(
    HexColor? Hue = null,
    double Lightness = 0d,
    double Saturation = 0d,
    double Gamma = 1d,
    bool InvertLightness = false,
    Visibility Visibility = Visibility.On,
    HexColor? Color = null,
    int Weight = 0)
{
    // Backing Fields
    
    private readonly double _lightness = Lightness;
    private readonly double _saturation = Saturation;
    private readonly double _gamma = Gamma;
    
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
            rules.Add($"lightness:{Lightness}");
        }
        
        if (Saturation is not 0d)
        {
            rules.Add($"saturation:{Saturation}");
        }
        
        if (Gamma is not 1d)
        {
            rules.Add($"gamma:{Gamma}");
        }

        if (InvertLightness)
        {
            rules.Add("invert_lightness:true");
        }
        
        if (Visibility is not Visibility.On)
        {
            rules.Add($"visibility:{Visibility}");
        }
        
        if (Color is not null)
        {
            rules.Add($"color:{Color.Value.ToString()}");
        }

        if (Weight != 0)
        {
            rules.Add($"weight:{Weight}");
        }

        return string.Join("|", rules);
    }
}