using Gravity.GoogleMaps.StaticMapBuilder.Models.MapStyles;
using Gravity.GoogleMaps.StaticMapBuilder.Types;

namespace Gravity.GoogleMaps.StaticMapBuilder.Tests.Models.MapStyles;

[TestSubject(typeof(MapStyle))]
public class MapStyleTest
{
    [Theory]
    [InlineData(
        "feature:administrative.country|element:labels.text.fill|color:0x00FF00",
        "administrative.country",
        "labels.text.fill",
        "0x00FF00")]

    [InlineData(
        "feature:road.local|color:0xFF00AA",
        "road.local",
        null,
        "0xFF00AA")]

    [InlineData(
        "element:geometry|color:0xFF00BB",
        null,
        "geometry",
        "0xFF00BB")]
    [InlineData(
        "color:0xFF00EE",
        null, 
        null,
        "0xFF00EE")]
    public void ToString_ProducesExpectedOutput(
        string expected,
        string? featureValue,
        string? elementValue,
        string? styleColor)
    {
        HexColor? color;
        if (styleColor is not null)
        {
            color = new HexColor(styleColor);
        }
        else
        {
            color = null;       
        }
        Feature? feature = featureValue is not null ? new Feature(featureValue) : null;
        Element? element = elementValue is not null ? new Element(elementValue) : null;
        StyleRule style = new(Color: color);

        MapStyle mapStyle = new(style, feature, element);

        string result = mapStyle.ToString();

        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToString_WithOnlyStyleRule_ReturnsStyleOnly()
    {
        StyleRule style = new(Color: new HexColor("0xFFCC00"));
        MapStyle mapStyle = new(style);

        string result = mapStyle.ToString();

        Assert.Equal("color:0xFFCC00", result);
    }
}