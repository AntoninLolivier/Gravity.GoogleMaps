using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Gravity.GoogleMaps.StaticMapBuilder.Enums;
using Gravity.GoogleMaps.StaticMapBuilder.Models;
using Gravity.GoogleMaps.StaticMapBuilder.Types;
using OneOf;

namespace Gravity.GoogleMaps.StaticMapBuilder.Tests.Models;

[TestSubject(typeof(CoordinatesMarker))]
public class CoordinatesMarkerTest
{

    [Theory]
    [InlineData(0, 0)]
    [InlineData(48.8566, 2.3522)]
    [InlineData(-90, -180)]
    [InlineData(90, 180)]
    public void Constructor_ValidCoordinates_DoesNotThrow(double lat, double lng)
    {
        CoordinatesMarker marker = new(lat, lng);
        Assert.Equal(lat, marker.Latitude);
        Assert.Equal(lng, marker.Longitude);
    }

    [Theory]
    [InlineData(-91)]
    [InlineData(91)]
    public void Constructor_InvalidLatitude_Throws(double lat)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new CoordinatesMarker(lat, 0));
    }

    [Theory]
    [InlineData(-181)]
    [InlineData(181)]
    public void Constructor_InvalidLongitude_Throws(double lng)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new CoordinatesMarker(0, lng));
    }

    [Fact]
    public void ToString_AnchorSetButNotIconUrl_Throws()
    {
        Assert.Throws<InvalidOperationException>(() => new CoordinatesMarker(0, 0, anchor: MarkerAnchor.Center).ToString());
    }
    
    [Theory]
    [InlineData(MarkerSize.Tiny)]
    [InlineData(MarkerSize.Small)]
    public void ToString_Throws_WhenLabelUsedWithTinyOrSmallSize(MarkerSize size)
    {
        CoordinatesMarker marker = new(10, 20, size: size, label: 'X');

        ArgumentException exception = Assert.Throws<ArgumentException>(() => marker.ToString());

        Assert.Contains("label", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Theory]
    [InlineData(10, 20)]
    [InlineData(-45.5, 120.25)]
    [InlineData(0, 0)]
    [InlineData(90, -180)]
    public void ToString_IncludesCoordinatesAndDollarPrefix(double lat, double lng)
    {
        CoordinatesMarker marker = new(lat, lng);
        string result = marker.ToString();

        Assert.EndsWith($"{lat.ToString(CultureInfo.InvariantCulture)}," +
                        $"{lng.ToString(CultureInfo.InvariantCulture)}", result);
    }
    
    [Theory]
    [InlineData("size:mid|color:0xFF0000|label:A|anchor:center|icon:http://icon.com|10.3,60.2",
        10.3, 60.2, MarkerSize.Mid, "0xFF0000", 'A', MarkerScale.One, "Center", "http://icon.com")]
    [InlineData(
        "size:mid|label:B|scale:2|10.9,-20.1",
        10.9, -20.1, MarkerSize.Mid, null, 'B', MarkerScale.Two, null, null)]
    [InlineData(
        "10,0",
        10, 0, MarkerSize.Default, null, null, MarkerScale.One, null, null)]
    [InlineData(
        "size:tiny|0,0",
        0,0, MarkerSize.Tiny, null, null, MarkerScale.One, null, null)]
    [InlineData(
        "0,0",
        0,0, MarkerSize.Default, null, null, MarkerScale.One, null, null)]
    [InlineData(
        "color:0xFFFFFF|0,0",
        0,0, MarkerSize.Default, "0xFFFFFF", null, MarkerScale.One, null, null)]
    [InlineData(
        "color:red|0,0",
        0,0, MarkerSize.Default, "Red", null, MarkerScale.One, null, null)]
    [InlineData(
        "label:A|0,0",
        0,0, MarkerSize.Default, null, 'A', MarkerScale.One, null, null)]
    [InlineData(
        "scale:4|0,0",
        0,0, MarkerSize.Default, null, null, MarkerScale.Four, null, null)]
    [InlineData(
        "anchor:topleft|icon:https://icon.com|0,0",
        0,0, MarkerSize.Default, null, null, MarkerScale.One, "TopLeft", "https://icon.com")]
    [InlineData(
        "anchor:5,4|icon:https://icon.com|0,0",
        0,0, MarkerSize.Default, null, null, MarkerScale.One, "(5,4)", "https://icon.com")]
    public void ToString_ReturnsExactExpectedString(
        string expectedEnding,
        double lat,
        double lng,
        MarkerSize size,
        string? color,
        char? label,
        MarkerScale markerScale,
        string? anchor,
        string? iconUrl)
    {
        OneOf<StaticMapColor, HexColor>? typpedColor;
        
        if (color is null)
        {
            typpedColor = null;
        }
        else
        {
            try
            {
                typpedColor = new HexColor(color);
            }
            catch (ArgumentException)
            {
                Dictionary<string, StaticMapColor> dictionary = Enum.GetValues<StaticMapColor>().ToDictionary(value => value.ToString(), value => value);
                typpedColor = dictionary[color];
            }
        }

        OneOf<MarkerAnchor, (int, int)>? typpedAnchor;

        if (anchor is null)
        {
            typpedAnchor = null;
        }
        else
        {
            if (anchor.Contains('('))
            {
                int[] coord = anchor.Trim('(', ')')
                    .Split(',')
                    .Select(int.Parse)
                    .ToArray();
                typpedAnchor = (coord[0], coord[1]);
            }
            else
            {
                Dictionary<string, MarkerAnchor> dictionary = Enum.GetValues<MarkerAnchor>().ToDictionary(value => value.ToString(), value => value);
                typpedAnchor = dictionary[anchor];
            }
        }
        CoordinatesMarker marker = new(lat, lng, size, typpedColor, label, markerScale, typpedAnchor, iconUrl);

        string result = marker.ToString();

        Assert.Equal(expectedEnding, result);
    }


}