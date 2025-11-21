namespace Gravity.GoogleMaps.StaticMapBuilder.Tests.Models.MapStyles;

[TestSubject(typeof(StyleRule))]
public class StyleRuleTest
{
    [Theory]
    [InlineData(-101, 0, 1)] // lightness too low
    [InlineData(0, -101, 1)] // saturation too low
    [InlineData(0, 0, 0.005)] // gamma too low
    [InlineData(101, 0, 1)] // lightness too high
    [InlineData(0, 101, 1)] // saturation too high
    [InlineData(0, 0, 10.1)] // gamma too high
    public void Constructor_Throws_WhenValuesOutOfRange(double lightness, double saturation, double gamma)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new StyleRule(
                Lightness: lightness,
                Saturation: saturation,
                Gamma: gamma
            ));
    }

    [Fact]
    public void ToString_ReturnsEmpty_WhenAllDefaults()
    {
        StyleRule rule = new();
        string result = rule.ToString();

        Assert.Equal(string.Empty, result);
    }

    [Theory]
    [InlineData("hue:0xFF0000", "0xFF0000", 0, 0, 1, false, Visibility.On, null, 0)]
    [InlineData("lightness:-50|saturation:40", null, -50, 40, 1, false, Visibility.On, null, 0)]
    [InlineData("gamma:2.5", null, 0, 0, 2.5, false, Visibility.On, null, 0)]
    [InlineData("invert_lightness:true", null, 0, 0, 1, true, Visibility.On, null, 0)]
    [InlineData("visibility:simplified", null, 0, 0, 1, false, Visibility.Simplified, null, 0)]
    [InlineData("color:0x0000FF", null, 0, 0, 1, false, Visibility.On, "0x0000FF", 0)]
    [InlineData("weight:4", null, 0, 0, 1, false, Visibility.On, null, 4)]
    public void ToString_ReturnsExpectedRuleStrings(
        string expected,
        string? hue,
        double lightness,
        double saturation,
        double gamma,
        bool invert,
        Visibility visibility,
        string? color,
        int weight)
    {
        HexColor? nullableHue;
        HexColor? nullableColor;
        if (hue is not null)
        {
            nullableHue = hue;
        }
        else
        {
            nullableHue = null;
        }

        if (color is not null)
        {
            nullableColor = color;
        }
        else
        {
            nullableColor = null;
        }
        StyleRule rule = new(
            Hue: nullableHue,
            Lightness: lightness,
            Saturation: saturation,
            Gamma: gamma,
            InvertLightness: invert,
            Visibility: visibility,
            Color: nullableColor,
            Weight: weight
        );

        string result = rule.ToString();

        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToString_ReturnsCombinedValues()
    {
        StyleRule rule = new(
            Hue: new HexColor("0xFF0000"),
            Lightness: 50,
            Saturation: -20,
            Gamma: 2.2,
            InvertLightness: true,
            Visibility: Visibility.Simplified,
            Color: new HexColor("0x00FF00"),
            Weight: 4
        );

        string result = rule.ToString();

        Assert.Equal(
            "hue:0xFF0000|lightness:50|saturation:-20|gamma:2.2|invert_lightness:true|visibility:simplified|color:0x00FF00|weight:4",
            result
        );
    }
}